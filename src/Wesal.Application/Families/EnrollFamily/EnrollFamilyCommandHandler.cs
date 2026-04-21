using Wesal.Application.Abstractions.PaymentGateway;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Families;
using Wesal.Contracts.Users;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.EnrollFamily;

internal sealed class EnrollFamilyCommandHandler(
    IParentRepository parentRepository,
    IFamilyRepository familyRepository,
    IUserService userService,
    IStripeGateway stripeGateway,
    IUnitOfWork unitOfWork) : ICommandHandler<EnrollFamilyCommand, EnrollFamilyResponse>
{
    public async Task<Result<EnrollFamilyResponse>> Handle(
        EnrollFamilyCommand request,
        CancellationToken cancellationToken)
    {
        if (await parentRepository.ExistsByNationalIdAsync(request.Mother.NationalId, cancellationToken))
            return FamilyErrors.ParentAlreadyExists(request.Mother.NationalId);

        var fatherUser = await userService.CreateAsync(UserRole.Parent, cancellationToken);
        var motherUser = await userService.CreateAsync(UserRole.Parent, cancellationToken);

        var father = Parent.Create(
            fatherUser.User.Id,
            request.CourtId,
            isFather: true,
            request.Father.NationalId,
            request.Father.FullName,
            request.Father.BirthDate,
            "male",
            request.Father.Address,
            request.Father.Phone,
            request.Father.Job,
            request.Father.Email);

        var mother = Parent.Create(
            motherUser.User.Id,
            request.CourtId,
            isFather: false,
            request.Mother.NationalId,
            request.Mother.FullName,
            request.Mother.BirthDate,
            "female",
            request.Mother.Address,
            request.Mother.Phone,
            request.Mother.Job,
            request.Mother.Email);

        var family = Family.Create(request.CourtId, father.Id, mother.Id);

        foreach (var childDto in request.Children ?? [])
        {
            var child = Child.Create(
                family.Id,
                childDto.FullName,
                childDto.BirthDate,
                childDto.Gender,
                childDto.SchoolId);

            family.Children.Add(child);
        }

        var fatherCustomerResult = await stripeGateway.CreateCustomerAsync(father, cancellationToken);
        if (fatherCustomerResult.IsFailure)
            return fatherCustomerResult.Error;

        var motherCustomerResult = await stripeGateway.CreateCustomerAsync(mother, cancellationToken);
        if (motherCustomerResult.IsFailure)
        {
            await stripeGateway.DeleteCustomer(fatherCustomerResult.Value);
            return motherCustomerResult.Error;
        }

        var fatherAccountResult = await stripeGateway.CreateConnectAccountAsync(father, cancellationToken);
        if (fatherAccountResult.IsFailure)
        {
            await stripeGateway.DeleteCustomer(fatherCustomerResult.Value);
            await stripeGateway.DeleteCustomer(motherCustomerResult.Value);
            return fatherAccountResult.Error;
        }

        var motherAccountResult = await stripeGateway.CreateConnectAccountAsync(mother, cancellationToken);
        if (motherAccountResult.IsFailure)
        {
            await stripeGateway.DeleteCustomer(fatherCustomerResult.Value);
            await stripeGateway.DeleteCustomer(motherCustomerResult.Value);
            await stripeGateway.DeleteAccount(fatherAccountResult.Value);
            return motherAccountResult.Error;
        }

        father.SetupStripe(fatherCustomerResult.Value, fatherAccountResult.Value);
        mother.SetupStripe(motherCustomerResult.Value, motherAccountResult.Value);

        await parentRepository.AddRangeAsync([father, mother], cancellationToken);
        await familyRepository.AddAsync(family, cancellationToken);

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception) // Catching generic exception (like DbUpdateException) to rollback Stripe
        {
            await stripeGateway.DeleteCustomer(fatherCustomerResult.Value);
            await stripeGateway.DeleteCustomer(motherCustomerResult.Value);
            await stripeGateway.DeleteAccount(fatherAccountResult.Value);
            await stripeGateway.DeleteAccount(motherAccountResult.Value);

            return Error.Validation("Family.EnrollmentFailed", "Failed to enroll family due to a database constraint violation.");
        }
        
        return new EnrollFamilyResponse(
            family.Id,
            new UserCredentialResponse(father.Id, father.NationalId, fatherUser.TemporaryPassword),
            new UserCredentialResponse(mother.Id, mother.NationalId, motherUser.TemporaryPassword));
    }
}
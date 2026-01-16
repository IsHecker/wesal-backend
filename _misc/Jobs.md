| Job | Frequency | Reason |
|-----|-----------|--------|
| GenerateVisitationSessionsJob | Daily (00:00) | Generates 30-90 days ahead |
| GenerateAlimonyPaymentDuesJob | Daily (00:00) | Generates 12 months ahead |
| DetectMissedVisitationsJob | Hourly | 30-min grace period needs timely detection |
| DetectOverdueAlimonyPaymentsJob | Every 6 hours | Due dates are daily, 4x/day is sufficient |
| SendAlimonyPaymentRemindersJob | Daily (09:00) | 7-day advance notice |
| SendVisitationRemindersJob | Daily (09:00) | 24-hour advance notice |
# 🔹 1. User Roles

**Important Note:** Zero self-registration in this system. Everything is court-initiated to maintain legal control.

### 1.1 Court Staff

**Who they are:**

- Government employees working in family court
- The **only** role (and Court admin) that can create structural data which maintains legal integrity and prevents fraud

**What they do in the system:** 

- **User & Families Management**
- **Schools Management:** create school records when needed for a case
- **Court Cases Management**
- **Visitation Schedules & Location Management:** define visitation rules and schedules based on court orders

## 1.2 Court Admin (Optional)
**Who is he:**

- The highest rank in the family court that manages everything below (maybe a court manager)
- Runs the business logic within the court system

**What he do in the system:**

- Everything Court Staff can do
- Manage system-level settings
- Configure payment gateways
- Configure their own family court rules
- Creates and assigns account to court staff
- Access system-wide reports and analytics

## 1.3 Parents
**Who they are:**

- Mothers and fathers involved in family court cases

**What they do in the system:**

1. **View-Only Access**
   - View custody decisions affecting them
   - See assigned visitation schedules
   - View their children's information
   - Check alimony payment obligations
   - Check scheduled/upcoming visitations
   - Read notifications and alerts
2. **Limited Actions**
   - Request visits
   - Request custody for some time
   - Make alimony payments online
   - File complaints (optional feature)
   - Receive system notifications (SMS/email)

**What they CANNOT do:**

- Create or modify families
- Change visitation schedules
- Edit children's information
- Access other families' data

## 1.4 Schools

**Who they are:**

- School administrators or staff members
- Representatives of educational institutions

**What they do in the system:**

1. **Limited Reporting Access**
   - Log in using court-issued credentials
   - View ONLY children assigned to their school
   - Upload student reports (attendance, grades, behavior)

**What they CANNOT do:**

- Access other schools' data
- View full family information
- Access court case details
- Create children or families

## 1.5 Platform Admin (Optional)

**Who they are:**

- E7na :)

**What they do:**

- Maintain the system
- Manage courts
- Configure system-wide settings

**What they do NOT do:**

- Touch any business-related stuff
- Make legal decisions
- Interact with parents or schools

## Role Separation & Security
- **Authentication:** Role-based access control enforced at every endpoint
- **Data Isolation:** Each isn't allowed to access other user's data at all
- **Audit Trail:** All actions logged with timestamp, user, and role for evidence


---

# 🔹 2. Core Entities

## 2.1 Families
### Definition

A single divorced family, which is a legally recognized group of individuals involved in a family court case.

### Why Is It Needed

- Families are the **unit of enforcement** in the system
- link parents, their children, and the court case supervising them
- Forming the root relationship
- Every court case decision, visitation, and payment is tied to a family

### Who Manages It

- Court staff Enrolls the family after receiving the case file

## 2.2 Court Cases

### Definition

An official legal case file that defines Legal decisions and rulings for a family

### Why Is It Needed

- Court cases are the **source of truth** for all rules
- Tracks everything legal about a family
- Cases provide legal traceability

### Who Manages It

- Court staff creates court cases during family enrollment

## 2.3 Custodies

### Definition

The court's ruling on which parent has primary custody of the children

### Why Is It Needed

- It connects a **child** to their Custodian parent and the period of custody
- It also stores the **court decision**, explaining why custody was given to the other parent
- This is important for proving who has the right to care for the children
- Directly affects visitation and alimony rules

### Who Manages It

- Court staff

## 2.4 Visitation Schedules

### Definition

Represents the Court-defined rules that define how visitations must happen legally over time and the expected pattern of visitation

### Why Is It Needed

- Required for generating visitation sessions automatically without manually creating every visit
- Provides **legal evidence** of what was ordered by the court

### Who Manages It

- Court staff creates schedules when the court issues or updates a visitation rule

## 2.5 Visitations

### Definition

A scheduled/planned visitation session where the non-custodial parent spends time with their children under court-approved rules

### Why Is It Needed

- It tracks when and where the non-custodial parent visits their child, and who verified it happened
- To confirm attendance of both non-custodial parent and the children
- To act as a legal proof for whether the parent showed up and  the visitation happened completely

### Who Manages It

- Automatically generated by the System

## 2.6 Visit Locations

### Definition

Court-approved places where visitations occur

### Why Is It Needed

- Provides **legal supervision** for visitations
- To ensure visits can only happen in **safe**, **approved**, and **recorded** locations
- Creates an **audit trail** (attendance logs)

### Who Manages It

- Court Staff or Court Admin

## 2.7 Alimony Payments

### Definition

The court-ordered financial support that one parent pays to the other (or for the children)

### Why Is It Needed

- To track financial obligations clearly and legally
- To act as legal proof for whether the payment was made, and includes a receipt link for evidence
- This ensures transparency and prevents false claims of unpaid or missing alimony

### Who Manages It

- Automatically generated by the System

## 2.8 Schools & Child Reports

### Definition

A **School** is an educational institution where a child from a divorced family is registered

### School Account

- **Username:** Auto-generated (e.g., `sch-cairo-0142` )
- **Password:** Maybe temporary, must be changed on first login
- **Access:** View only children assigned to their school

### Why Is It Needed

- To identify where each child studies, allowing the court and parents to request school reports or any data directly

### Who Manages It

- Court staff creates Schools during family enrollment
- School account is generated by the System automatically 

## 2.9 Obligation Alerts & Violations

### Definition

Automated notifications triggered when a parent fails to comply with their obligations

### Why Is It Needed

- To automatically send notifications to both the court and the affected parent
- It helps the system enforce compliance when a parent doesn’t follow a court order

### Who Manages It

- Generated automatically by the system when a parent fails with their obligations

## 2.10 Notifications

### Definition

Tracks system-generated messages sent to users that informs them of important events

### Why Is It Needed

- To handle communication between the system and parents: visitation reminders, missed payment warnings, or updates from the court

### Who Manages It

- The System creates notifications when some event happens

## 2.11 Complaints (Optional Feature)

### Definition

Official complaints filed by a parent alleging a violation by the other parent

### Why Is It Needed

- To give parents a legal way to report violations that aren't handled by the system
- Each complaint can be linked to the related family, and sent to the **court** for review and possible action.
- Parental disputes history

### Who Manages It

- Parent files these complaints
- Court staff reviews these complaints

# 🔹 3. Data & Action Flows

## 3.1 ✅Family Enrollment
**Actors:** Court Staff
**Trigger(Outside):** Court receives physical case file from legal proceedings

**Flow:**

1. Staff navigates to "Create New Family"
2. Staff creates **Parent A** user account:
    - Full name, national ID, phone, address
    - Parent Role: Father or Mother
    - System generates temporary password

3. Staff creates **Parent B** user account (same process)
4. Staff creates **Child Profile(s)**:
    - Children are subjects, not users

5. Staff creates **Family Record**:
    - System automatically create family record and links family to a **Court Case ID**

**Output:** Verified family record with all members linked to current case

**Legal trust:** Only authorized court staff can create families

**No fraud:** Parents cannot self-register or create fake accounts

## 3.2 ✅Court Case Setup
**Actors:** Court Staff

**Trigger:** Court case file contains custody and visitation orders

**Flow:**

1. Staff navigates to **"New Court Case"**
2. Staff enters **case metadata**
3. Staff links case to **Family ID** (created in `Family Enrollment`)
4. Staff enters **Custody Decision**:
    - Upload court order document**(Optional)**

5. Staff defines **Visitation Schedules**
6. Staff optionally defines **Alimony Obligations**
7. System **validates** all data and saves the case

## 3.3 ✅Parent Interactions
**Actors:** Parents

**What Parents Can View:**

- Basic information about their family and children
- Custody decision affecting them
- Current Court case status and details
- Full visitation schedule (upcoming and past)
- Alimony obligations/schedules, Payment history and receipts for the non-custodial
- Notifications and alerts
- Receipts history for evidence

**What Parents Can Do:**

1. **Make Alimony Payments (non-custodial parent)**
    - Click "Pay Now"
    - Redirected to payment gateway
    - Complete payment
    - Receive confirmation email + receipt

2. **File Complaints (Optional Feature)**
    - Report denial of visitation
    - Report harassment
    - Upload evidence (photos, screenshots)


**Parents CANNOT:**

- Modify any information
- See other families' data

**UI Example (Parent Dashboard):**

```
Welcome, Ahmed Hassan

Your Upcoming Visitations:
- Jan 6, 2025, 3:00 PM at Family Center Cairo
- Jan 13, 2025, 3:00 PM at Family Center Cairo

Alimony Due: $500 (Due: Jan 1, 2025)
[Pay Now]

Notifications: 2 unread
```

## 3.4 ✅Automatic Visitation Generation
**Actors:** System (background job)

**Trigger:** Court case setup completes with **Visitation Schedules**

**Flow:**

1. System reads defined **Visitation Schedules**
2. System calculates **next month/12 months** of visitation dates:
    - Creates individual **Visitation** records based on frequency

3. System saves all **Visitation** sessions to database
4. System sends **notification** to non-custodial parent:
    - "Your visitation schedule is now available"
    - Lists upcoming 3-4 visits

## 3.5 ✅Visitation Attendance Recording

**Actors:** Visit Center Staff, System

**Trigger:** Scheduled visitation date arrives

**Flow:**

1. **Before Visit:** System sends a **reminder notification** to parent 24 hours before
2. **On Visit Day:**
   1. Parent arrives at visit location 
   2. Center staff opens system and navigates to **"Today's Visitations"** screen
   3. Staff tries to find the scheduled session for this parent (maybe using **national ID**)
   4. Staff clicks **"Check In"**
   5. System records timestamp
   6. Status changes to `Checked-In`  
3. **After Visit:** 
   1. Staff marks the visitation as `Completed` when parent leaves
   2. System records end time and changes status to `Completed` 

**Output:**

- Attendance recorded
- Data becomes **legal evidence**

## 3.6 ✅Alimony Payment Flow

**Actors:** Payer Parent, System, Payment Gateway, Court

**Trigger:** Monthly alimony due date approaches

**Flow:**
1. Can pay Alimony at any time before due date
2. **Before Due Date:**
    - System calculates next payment due date
    - System sends a **reminder notification** (7 days) before: 
      - Notification **(all type of ways)**: "Your alimony payment of $500 is due on Jan 1, 2025"
      - Email with payment link **(optional)**
3. **Payment Day:**
    - Parent logs into system
    - Navigates to **"Alimony"**
    - Clicks **"Pay Now"**
4. **Payment Processing:**
- Gateway processes transaction and sends a **Webhook** to the system
5. **System Records Payment:**
    - If **successful**, System:
      - logs the **Payment**
      - Marks alimony as `Paid`
      - sends **confirmation email** with receipt
    - If **failed**, System:
      - logs failure reason
      - sends a **notification** to the parent to try again later


**Output:**

- Both parents see payment history
- Transactions logged for legal proof

## 3.7 ✅School Creation & Child-School Linking

**Actors:** Court Staff, System, School

**Trigger:** Family case involves children enrolled in schools

**Flow (During Family Enrollment):**

1. Staff checks what school is this child in
   - This info is provided in the physical case file
2. Court staff checks if the school exists in the system
3. If school does NOT exist:
   - Court staff navigates to **"Register School"**
   - Enters school details
   - System saves **School Record**
   - System **auto-generates** school account:
     - Username: `sch-cairo-0142`
     - Temporary password: `X7!kQ9@P`
   - System displays school record with account details to the court staff
   - Staff writes down credentials securely
4. Staff then links child to school and saves relationship
   
5. Court contacts school via official channels **(Offline Communication)**:
   - Phone call or official letter
   - Provides credentials
   - Explains system purpose

**Output:**

- Schools are created **by court**, not self-registered
- School accounts are **institutional**, not personal
- Schools see **only children assigned to them**
- Schools **cannot** see families, cases, or other schools
- Court verifies schools before creation which prevents impersonation

## 3.8 ✅School Report Upload

**Actors:** School, System, Court Staff

**Trigger:** School wants to report child's status

**Flow:**

1. After school has officially received credentials offline, they can log in to the system
   - System forces **password change** on first login **(Optional)**

2. Dashboard shows only their students from divorced families linked to their school
3. Clicks on a student and navigates to **"Upload Report"**
4. School enters: 
   - Report type (attendance, grades, behavior)
   - Notes/description
   - Upload report file
5. School uploads
6. System stores the report and notifies both court staff and parents
7. Court staff reviews report **(Off-system)**: 
   - May affect custody evaluation
   - May trigger custody modification hearing
8. **Schools CANNOT:**
   - See full family information
   - View other schools' data
   - Create children or families

## 3.9 ✅Violation Detection & Alert Notification

**Actors:** System, Court Staff

**Trigger:** Parent fails to meet obligation

**Types of Violations:**

1. ✅**Missed Visitation**
    - System gives the parent a grace period of 30 minutes past scheduled time
    - Detection: Parent still doesn't show up
    - Action:
      - System marks the parent as `Absent`
      - Generates violation alert immediately


2. ✅**Unpaid Alimony**
    - A Job checks all alimony obligations with passed due dates
    - Detection: Due date passes and alimony isn't paid yet
    - Action: 
      - System marks the alimony record as `NotPaid`
      - Generates a **Violation alert**
    
3. **Custody Breach (Optional)**
    - Detection: Custodial parent denies access
    - Action: Non-custodial parent files complaint for the court to review it

**Notification Flow:**

1. Sends notification after alert creation to **court staff** and **Payer parent**
4. Court staff reviews alert: 
   - Marks alert as `Under Review`
   - Takes action
   - Adds resolution notes
   - Finally marks alert as `Resolved`

**Output:** Violations are tracked automatically and no manual monitoring is needed

---

# 🔹 4. Off-system & External Communications

### 1. Court ↔ Parents

- Court hearings/sessions (in-person or virtual)
- Verbal court decisions
- Paper documents
- Legal summons
- Lawyer consultations
- Mediation sessions

- **What could be on the System:**

  - Records the results after they happen

  - Stores custody decisions after court rules

  - Logs visitation rules after court defines them
    


### 2. Court ↔ Schools

- Court calls school to notify them of system access
- Court sends official letter with credentials
- Court emails school administration
- Physical delivery of login information
  

### 3. Parents ↔ Parents

- Verbal agreements between parents
- Arguments or disputes
- Text messages between parents
- Phone calls
  

### 4. Parents ↔ Schools

- Parent-teacher meetings
- Phone calls about child's progress
- School events and activities
- Academic counseling sessions

- **What could be on the System:**

  - Stores **only official reports** uploaded by schools

  - Ignores informal conversations
    


## System Responsibilities

**It Should:**

- Records official Court decisions
- Notifies users
- Proves violations with data
- Enforce rules automatically
- Tracks legal obligations

**What It Doesn't Do:**

- Make legal decisions
- Negotiate between parties
- Contact people manually
- Enforce physically (that's law enforcement's job)
- Replace court hearings

---

# 🔹 5. MVP Scope

### 1. User Management by Court admins and staff only

### 2. ✅Family Enrollment

### 3. ✅Court Case Setup

### 4. ✅Automatic Visitation Generation

### 5. ✅Automatic Violations Detection

### 6. ✅Visitation Attendance Recording

### 7. ✅Parent Dashboard

   - Parents can log in with National ID

   - Parents see their assigned data: 
     - Family information
     - Court case summary
     - Upcoming and past visitations
     - Alimony obligations and payment history

   - Parents can view notifications

### 8. ✅Multi-Court & Court Isolation Support

   - Each court operates independently
   - Courts cannot see each other's data
   - Platform Admin creates new court instances and account

   ### **Principles**

   - All structural and important data created by court staff
   - System focuses mostly on enforcing rules
   - Every action is logged for legal traceability
   - Strict isolation between different roles through Role-Based Access
   - Automation for workflows/tasks as much as possible

---

# 🔹 6. Beyond MVP

### 4. ✅Document Management (Optional Feature)

- Document Uploaded by any role
- **Document Storage:**
  - Secure file storage
  - Audit trail (who uploaded, when)
- **Document Viewing:**
  - Court staff view all documents per case
  - Parents view documents relevant to them
  - Schools view documents for their students

### **1. ✅Alimony Enforcement**

- **Obligation Management:**
  - Court staff define alimony terms
  - System calculates due dates automatically and track payment status
- **Payment Processing:**
  - Integrate payment gateway
  - Parent portal shows "Pay Now"
  - System records transaction confirmations
  - Generate payment receipts
- **Alimony Violations when parent missed payments**
- **Payment History**

### 2. ✅School Integration

- ✅School Account Management
- ✅Child-School Linking

### 3. ✅Custody Request

- **The Non-Custodial parent issues a request for custody:**
  - Specify the period of time for this custody
  - Reason for this request
- **Court:**
  - Reviews the custody request
  - Can Accept/Decline the request with the court decision note
  - Update custody decision with effective date

### 5. ✅Complaint System (Optional Feature)

- **Parent Complaints:**
  - Parents file complaints against other parent
  - Complaint Type
  - Upload evidence
- **Court staff:**
  - Review parent complaints
  - Investigate and gather more info
  - Mark complaint as resolved with notes
- **Complaint History:**
  - Track all complaints per case
  - Use in custody modification hearings

### 6. ✅Compliance Analytics (Optional Feature)

- Visitation attendance rates

- Alimony payment on-time rates
- Violation rate over time
- **Export Analytics Reports as PDF** which can be used for court hearings


---

# Confusion Points

- Should there be multiple school accounts under the main school account just like court admin account that can create court staff accounts.

---

# Father app view

- Visitation Page
  - Remove visitation cancellation button
  - Remove attendance 15 min before message
  - Display only the upcoming visitation, not the whole list
  - Remove Request new visitation
  - Remove Custody request outside the visitation page

- Alimonies Page
  - Remove late alimonies warning(not supported)
  - Display only the upcoming payment due, not the whole list
  - Remove payment history page, and display it directly in the alimonies page
  - List payment history when clicking on the payment due

- Home Page
  - Display the whole family details, not just the children

- School Reports Page
  - List reports as downloadable pdf records instead.
  - Reports should be listed under a specific child for organization.

- Court Case Page
  - In Custody details section, add button for custody request, and button to see request history

- Add Complaints Page
  - Display complaints history

- Account Page
  - Edit profile is directly without requests, and is only for: Email, Address, Phone, and Job
  - Remove complaints button

# Mother app view

- Everything is the same as father app view with some changes
  - Alimony Page 
    - Withdraw paid alimonies instead of paying
    - Provide a button for onboarding to provide bank details to withdraw money when it's not configured yet.

# School web view

- Login Page
  - Login using username and password
- Home Page
  - List children in the school
- Child detail Page
  - List uploaded reports history
  - Provide the ability to upload report
- Account Page
  - Only update contact info: Email and phone number
### **Users Table**
Represents every individual who interacts with the system — parents, judges, school staff, or children. Its purpose is to unify identity across all operations so every action (payment, visit, report, complaint) can be legally tied to a verified, nationally identified person.

### **Families Table**
Represents a single divorced family unit managed under the system. Its purpose is to link both parents, their children, and the court case supervising them — forming the root relationship around which all other records revolve (visits, payments, custody).

### **Children Table**
Represents a child within a family. Its purpose is to provide a distinct identity for each child so the system can track which custody applies to, who attends visits, and who receives school updates.

### **FamilyCourts Table**
This represents actual family courts across Egypt (like “Heliopolis Family Court”). It defines where each case is being handled. Courts differ by **governorate** and **location**, so this table makes it easy to organize cases, judges, and reports by region. It’s useful for official coordination between courts, judges, and institutions like schools or payment banks.

### **CourtCases Table**
Each record here is a real legal case file. It tracks everything legal about a family. This is the digital version of a physical court file. It lets the system know what legal phase a family is in (divorce, custody decision, visitation enforcement, etc.).

### **Judges Table**
Each record represents a judge who works in a family court.
**Purpose:**
To assign judges to cases and track who issued which decision. It helps maintain transparency and ensure accountability — for example, knowing which judge signed off on a custody or payment decision.

### **Custodies Table**
This table records who currently has custody of a child.
**Purpose:**
It connects a **child** to their **custodian (usually a parent)** and the period of custody. It also stores the **court decision**, explaining why custody was granted. This is vital for proving who legally has the right to care for the child and for tracking changes when the court updates custody arrangements.

### **Visitations Table**
Each record represents one planned or completed visitation session.
**Purpose:**
It tracks when and where the non-custodial parent visits their child, and who verified it happened. The **VerifiedBy** field is crucial — it’s the official confirmation that the visit occurred properly. In Egypt, visitations are usually supervised in designated locations (like youth centers or social service facilities), and staff confirm attendance — this record acts as legal proof.

### **VisitLocations Table**
This lists all approved places where visitations can happen.
**Purpose:**
To ensure visits only happen in **safe**, **approved**, and **recorded** locations. In Egypt, these are often **fixed public or government-supervised centers**.

### **AlimonyPayments Table**
This table logs every child-support payment made by one parent to the other.
**Purpose:**
To track financial obligations clearly and legally. It records when payments were due, if they were made, through which method (Stripe or bank), and includes a **receipt link** for verification. This ensures transparency and prevents false claims of unpaid or missing alimony.

### **ObligationAlerts Table**
This table records alerts when something goes wrong — like a missed visit or an unpaid alimony.
**Purpose:**
To automatically flag problems to both the **court** and the **affected parent**. It helps the system enforce compliance: when someone doesn’t follow a court order, this table logs the issue, tracks whether the parent and court were notified, and how serious it is. This creates an automated early-warning system for violations.

### **Schools Table**
This table stores information about schools registered in the system.
**Purpose:**
To identify where each child studies, allowing the court to request school reports or attendance data directly. It helps create a link between the child’s education and the court’s monitoring system.

### **ChildReports Table**
This table stores uploaded reports about a child from their school.
**Purpose:**
To let schools upload behavior or performance reports directly to the system. Judges or custodians can view these documents to understand the child’s condition, ensuring that custody and visitation decisions consider the child’s well-being.

### **Notifications Table**
This table tracks messages and alerts sent to users.
**Purpose:**
To handle communication between the system and parents — reminders about visits, warnings about missed payments, or updates from the court. It helps automate communication instead of relying on manual phone calls or papers.

### **Complaints Table**
This table stores official complaints made by parents.
 **Purpose:**
 To give parents a structured, legal way to report violations, such as a missed visit or unpaid alimony. Each complaint can be linked to the related **visit**, **payment**, or **family**, and sent to the **court** for review and possible action. It formalizes the process and keeps records of all parental disputes.

### **PaymentInstitutions Table**
This table represents all financial entities (banks or online gateways) that process alimony payments.
**Purpose:**
To track where payments come from — for example, through **Nasser Social Bank** or **Stripe**. This adds an extra verification layer and ensures that financial records can be validated officially if needed by the court, and lets the system track which organization processed the payment.


# ✅ **System Resources & Responsibilities**
## **1. Users (Parents, Child Profiles, Court Staff, School Officials, Admins)**
**Managed by:** Court Staff + Admins
 **Actions:**

- Create parent user accounts
- Create child profiles and link them to families
- Assign system roles
- Update user details (address, phone, etc.)
- Disable/archive accounts
Parents cannot create accounts themselves—you want legal verification.

## **2.  Families**
**Managed by:** Court Staff
 **Actions:**

- Create a new family record
- Assign father ↔ mother ↔ children
- Link the family to a court case
- Update when the case changes
- Review family details
This forms the backbone of the project.

## **3.  Court Cases**
**Managed by:** Court Staff + Family Court Admins
 **Actions:**

- Register court case info
- Assign families
- Store decisions
## **4.  Custody Decisions**
**Managed by:** Court Judges / Staff
 **Actions:**

- Assign the custody holder
- Modify or terminate custody
- Store court decision documents
This is crucial for legal validation.

## **5.  Visitations**
**Managed by:** Court + Visit Location Staff
 **Actions:**

- Schedule visitation sessions
- Approve or reject dates
- Check in parents on arrival
- Verify attendance
- Mark attendance status
This is one of the core operations.

## **6.  Visit Locations**
**Managed by:** Supervisors at visitation centers
 **Actions:**

- Add new location (court-approved)
- Update address, contact info
- Mark capacity limits
## **7.  Alimony Payments**
**Managed by:** System + Bank API + Court
 **Actions:**

- Generate payment requests
- Record payment status from Stripe
- Notify the court if the payment is missed
- Store financial proof
Parents only pay—they don’t “manage”.

## **8.  Obligation Alerts**
(Missed visit, unpaid alimony, custody breach)

**Managed by:** System (automated) + Court staff (manual resolution)
 **Actions:**

- Auto-generate alerts based on data
- Notify parents + court
- Track alert resolution
## **9.  Schools / Child Reports**
**Managed by:** School Officials
 **Actions:**

- Upload student reports
- Notify the family court
- View the child's identity assigned by the court
Parents do not interact here (to avoid fakes).

## **10.   Notifications**
**Managed by:** System automated service
 **Actions:**

- Send reminders
- Send alerts
- Track read/unread state
- Delivery method SMS/email
## **11.  Complaints**
**Managed by:** Parents + Court Staff
 **Actions:**

- Parents report violations (optional)
- Court reviews complaint
- Court marks resolution
## **12. Payment Institutions**
**Managed by:** Admin
 **Actions:**

- Register financial gateways
- Link gateways to payments
- Update configuration
# 📌 **Roles & What They Control**
| Role | What They Can Manage |
| ----- | ----- |
| **Court Staff** | users, families, cases, custody, visitations |
| **Court Admin** | all + system-level settings |
| **Parents** | view visits, view data, make payments |
| **School Officials** | upload reports about children |
---

# DATA FLOW


## **1.  Family Enrollment → User Creation**
**Actors:** Court Staff → System DB

**Flow:**

1. Staff receive a real family case file IRL.
2. Staff logs into the system and: 
    - Creates parent A user
    - Creates parent B user
    - Creates child profiles

3. They assign the parents + kids into a **Family record**.
4. The system connects the Family to a **Court Case** entry.
**Output:**

- Verified family record
- Only system staff can do this
This builds legal trust—you avoid fake data and fraud.



## **2.  Case Setup (Custody + Visit Rules)**
**Actors:** Court Staff (not the system)

**Flow:**

1. Staff enters custody decision: 
    - who holds full custody
    - Who gets visitation dates

2. Staff defines visitation rules: 
    - weekly/monthly schedule
    - time slots
    - allowed location

**Output:** Legal visitation framework.



## **3.  Visitation Scheduling**
This is the first piece where automation works.

**Flow:**

1. A scheduler runs on the system.
2. For each case → generate future visit dates.
3. Store these as visitation events.
Example:

- Every Saturday at 3 PM at center #4
Parents don’t choose dates → the court dictates them.



## **4.  Parent Interaction**
Parents can:

- View visitations
- Confirm attendance
- Make alimony payments
But cannot:

- Create families
- Change visits
- Change kids
- Change schools


## **5.  Visitation Day**
**Actors:** Center attendant + System

Data flow:

1. Parent arrives → check-in
2. System marks attendance: 
    - attended
    - absent

3. If the parent is absent, the System generates a violation event → reports to the court.
This is crucial. Data here becomes legal evidence.



## **6.  Alimony Payment Path**
**Flow:**

1. The system calculates monthly dues.
2. Parent pays online.
3. Payment confirmation comes from the gateway.
4. System logs payment success/failure status.
5. Missed payment → violation → court notified.


## **7.  School Creation & Child–School Linking (Court-Driven)**
**Actors:** Court Staff → System → School Official (later)

**Flow:**

1. **Family is created first**
    - Parents and children are registered from the legal case file.

2. **Court asks (or already knows): “Which school does this child attend?”**
This information already exists in the real-world court documents.
3. **If the school does NOT exist in the system yet:**
    - Court staff **creates the School record**.
    - This is **NOT** the school's self-registering.
    - This is controlled, legal data entry.

4. **The child record is then linked to the School**
    - `Children.SchoolId`  is assigned **after** the school exists.

5. **System generates a School Account**
    - Credentials are created by the system.
    - The court officially contacts the school and hands over access.

6. **School logs in later ONLY to:**
    - Upload reports for **children already linked to them**
    - They cannot create children, families, or schools.

**Key Rule (Very Important):**

> Schools only exist in the system **because a court case requires them**
Not because you’re building a national school registry.

**Why this works legally and technically:**

- No impersonation
- No fake schools
- No orphan children
- No circular dependency


## **8.  School Data Flow (Later MVP+)**
1. School uploads student attendance or grades.
2. The court reads and evaluates the behavior of both parents.
3. The court may modify custody.


## **MVP SCOPE**
Only build these 4 domains:

### **1. Staff → Create Families**
Core tables:

- users
- family
- family_member_type (Father, Mother, Child)
UI screens:

- Create user
- Create family
- Attach members
This is the absolute foundation.



### **2. Create Case + Custody + Visitation Rules**
UI:

- New case form
- Choose the primary custody holder
- Choose visitation frequency
Backend logic:

- Save these rules


### **3. Automatic Visitation Generation**
This is your core automation.

Logic:

- Given frequency (e.g., weekly), generate visitation events
Tables:

- visitation_schedule
- visitation_event


### **4. Attendance Recording**
Must have:

- check-in system
- parent attendance status
- violation event generation
This completes the legal chain.



# ⛔ MVP DOES NOT NEED:
- user self-signup
- school features
- alimony payments
- complex dashboards
- AI
- chat
- notifications
- reports
- analytics
- roles beyond simple (staff/parent)
Drop all of that to avoid drowning.



# MVP Data Flow in Order:
1️⃣ Court staff member signs in
2️⃣ Staff create a family
3️⃣ Staff create a custody case
4️⃣ Staff defines visitation rules
5️⃣ System generates recurring visit events
6️⃣ Parents log in to see their visits
7️⃣ Staff mark attendance
8️⃣ System stores violations



# After MVP / Phase 2
After your MVP works in a real courtroom simulation:

### Add:
- alimony logic
- school integration
- document uploads
- automatic reporting
- messaging
- mobile app
- push notifications
- data analytics
- lawyer access




# **Admins**
### 1️⃣ **You (system owner)**
- You are the **platform admin / super admin**
- Responsibilities:
    - Deploy the system
    - Manage courts as institutions
    - Handle outages, bugs, and configuration
    - **You do NOT touch families, cases, children, or schools**

Think of yourself like:

> Microsoft managing Windows — not deciding court cases.

---

### 2️⃣ **The Court (system admin/business admin)**
- Courts are the **operational admins**
- Responsibilities:
    - Register families
    - Register children
    - Assign schools
    - Create school accounts
    - Record rulings, visitation, and alimony
    - Activate/deactivate access

Think of them like:

> HR using an HR system — they run the business data.





Alright, here’s **how you actually achieve this in practice**, step by step, no fluff.

---

## 1️⃣ School accounts are **system accounts**, not real people
You are **not** modeling a human user here.
You are modeling an **institution login**.

So technically:

- A school gets **one account** (or a few later if needed)
- That account represents the **school as an entity**
---

## 2️⃣ How the school account is created (who + how)
### ✅ Who creates it?
**The family court staff (Admin role)**

Because:

- Courts are the authority
- Schools don’t self-register
- This prevents fake schools/impersonation
---

### ✅ How it’s created (backend flow)
When the court adds a child and selects their school:

1. Court admin checks:
    - “Is this school already in the system?”

2. If **NO**:
    - The court creates a **School record**
    - Court creates a **SchoolAccount (User with role = School)**

3. System auto-generates:
    - `username` 
    - `temporary password` 

---

## 3️⃣ How usernames are generated (automatically)
You **do not ask the school for it**.
The system generates it.

Examples:

### Option A – Sequential ID
```text
school-000142
```
### Option B – Based on the governorate
```text
sch-cairo-0142
```
### Option C – Ministry code (best if available)
```text
MOE-GIZA-ALN-0098
```
➡️ This is just a **string field** in the Users table.

---

## 4️⃣ Password handling (important)
- The system generates a **temporary password**
- Stored **hashed** (bcrypt / argon2)
- On first login:
    - The school is forced to change the password

Example:

```text
Username: sch-cairo-0142
Temp password: X7!kQ9@P
```
---

## 5️⃣ How does the school receive the credentials?
This part is **outside your system**, and that’s OK.

Possible real-life methods:

- Official letter from the court
- Phone call
- Secure government email (court → school admin)
- Physical delivery
👉 **Your system does NOT handle this communication**
👉 Your system only **assumes the school got the credentials**

---

## 6️⃣ How does the school log in?
Simple:

```text
POST /login
{
  "username": "sch-cairo-0142",
  "password": "new-password"
}
```
No email.
No signup.
No MFA (for MVP).

---

## 7️⃣ How the school knows what to upload
When the school logs in:

- They only see:
    - Children are assigned to **their school**

- This is enforced by: Children.SchoolId = School.Id
So UI shows:

> “Children under your school – upload reports”

They **cannot**:

- See other schools
- See families directly
- See court data






## ✅ Communications **INSIDE** Your System (Your Responsibility)
These are **digital, logged, auditable, and enforced by the system**.

### 1️⃣ Court ↔ System
- Court staff create and manage:
    - Families
    - Parents
    - Children
    - Court cases
    - Custody decisions
    - Visitation schedules
    - Alimony obligations

- Court staff register:
    - Schools (only when needed)
    - Visit locations

- Court staff issue:
    - System accounts (parents, schools)

- Court staff verify:
    - Visitations
    - Violations

- System sends:
    - Alerts
    - Notifications
    - Reports
    - Legal logs

👉 **This is the backbone of the system.**

---

### 2️⃣ Parents ↔ System
- Parents can:
    - View custody decisions
    - View visitation schedules
    - Confirm attendance (request verification)
    - View alimony obligations
    - Pay alimony
    - Receive notifications

- System records:
    - Missed visits
    - Late payments
    - Violations

👉 Parents **do not manage structure**, only interact with assigned data.

---

### 3️⃣ School ↔ System
- Schools can:
    - Log in using court-issued credentials
    - View **only children assigned to them**
    - Upload school reports

- System:
    - Locks access to unrelated families
    - Logs uploads for legal traceability

👉 Schools are **data providers**, nothing more.

---

### 4️⃣ System ↔ Payment Gateway (External API, but System-Controlled)
- System:
    - Initiates payments
    - Verifies payment status
    - Stores transaction proofs

- Payment gateway:
    - Processes money
    - Returns results

👉 This is **technical integration**, not human communication.

---

### 5️⃣ System ↔ Notification Services
- System sends:
    - SMS
    - Email
    - In-app notifications

- Delivery services **do not understand the domain**
- They just deliver messages
👉 System remains the **source of truth**.

---

## ❌ Communications **OUTSIDE** Your System (Not Your Responsibility)
These **must NOT be modeled as tables, APIs, or logic**.

### 1️⃣ Court ↔ Parents (Offline / Legal)
- Court hearings
- Verbal decisions
- Paper documents
- Legal summons
👉 Your system **records results**, not the conversation.

---

### 2️⃣ Court ↔ Schools (Initial Contact)
- Phone calls
- Official letters
- Emails from court administration
👉 Your system only cares **after** access is issued.

---

### 3️⃣ Parents ↔ Parents
- Agreements
- Arguments
- Verbal coordination
- Disputes
👉 If it’s not legally recorded, **it doesn’t exist in your system**.

---

### 4️⃣ Parents ↔ Schools
- Meetings
- Phone calls
- Academic discussions
👉 Only **uploaded reports** matter.

---

### 5️⃣ Child ↔ Anyone
- Children never use the system
- No accounts
- No actions
👉 Children are **subjects**, not actors.

---

## 🧠 Mental Model (Remember This)
Think of your system like a **digital court clerk**:

- It **records**
- It **verifies**
- It **notifies**
- It **proves**
It does **NOT**:

- Negotiate
- Decide
- Contact people manually
- Enforce physically

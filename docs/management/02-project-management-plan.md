# Project Management Plan - English Training Center Management System

## 1. Work Breakdown Structure (WBS)

```
ETCMS Project
├── Phase 1: Planning & Design
│   ├── 1.1 Requirements Analysis
│   ├── 1.2 System Architecture Design
│   ├── 1.3 Database Design
│   ├── 1.4 API Design
│   └── 1.5 Project Setup & Team Kickoff
│
├── Phase 2: Development
│   ├── 2.1 Infrastructure Setup (.NET Core, SQL Server)
│   ├── 2.2 Authentication & Security Module
│   ├── 2.3 Student Management Module
│   ├── 2.4 Course & Class Management Module
│   ├── 2.5 Instructor Management Module
│   ├── 2.6 Payment Management Module
│   ├── 2.7 Evaluation & Grading Module
│   ├── 2.8 Reports & Analytics Module
│   └── 2.9 Integration Testing
│
├── Phase 3: Testing & Quality Assurance
│   ├── 3.1 Unit Testing
│   ├── 3.2 Integration Testing
│   ├── 3.3 System Testing
│   ├── 3.4 Performance Testing
│   ├── 3.5 Security Testing
│   └── 3.6 UAT (User Acceptance Testing)
│
├── Phase 4: Deployment & Go-Live
│   ├── 4.1 Production Environment Setup
│   ├── 4.2 Data Migration
│   ├── 4.3 User Training & Documentation
│   ├── 4.4 System Deployment
│   ├── 4.5 Post-Launch Support
│   └── 4.6 Performance Monitoring
│
└── Phase 5: Post-Implementation
    ├── 5.1 Bug Fixes & Optimization
    ├── 5.2 User Support & Training
    ├── 5.3 System Maintenance
    └── 5.4 Feature Enhancement Planning
```

## 2. Project Timeline (Gantt)

### 2.1 High-Level Timeline
| Phase | Duration | Start Date | End Date |
|-------|----------|-----------|----------|
| **Planning & Design** | 3 weeks | Jan 28, 2025 | Feb 18, 2025 |
| **Development** | 10 weeks | Feb 19, 2025 | Apr 30, 2025 |
| **Testing & QA** | 3 weeks | May 1, 2025 | May 21, 2025 |
| **Deployment & Go-Live** | 2 weeks | May 22, 2025 | Jun 4, 2025 |
| **Post-Implementation** | Ongoing | Jun 5, 2025 | Ongoing |

### 2.2 Detailed Timeline

#### Phase 1: Planning & Design (3 weeks)
```
Week 1 (Jan 28 - Feb 4, 2025)
├── Finalize Requirements & Scope
├── Create System Architecture Design
├── Design Database Schema & ERD
└── Team Kickoff Meeting

Week 2 (Feb 5 - Feb 11, 2025)
├── API Design & Documentation
├── UI/UX Design (if applicable)
├── Infrastructure Planning
└── Technology Stack Finalization

Week 3 (Feb 12 - Feb 18, 2025)
├── Development Environment Setup
├── Database Schema Finalization
├── Create Development Standards & Guidelines
└── Project Repository Setup (Git, CI/CD)
```

#### Phase 2: Development (10 weeks)
```
Week 1-2 (Feb 19 - Mar 4, 2025): Foundation
├── .NET Core Project Structure Setup
├── Database Context (EF Core) & Migrations
├── Authentication & Authorization Module
├── Base Repository & Service Classes
└── Unit Testing Framework Setup

Week 3-4 (Mar 5 - Mar 18, 2025): Student Module
├── Student Entity & Repository
├── Student Service & API Controllers
├── Student Management Features
└── Unit & Integration Tests

Week 5-6 (Mar 19 - Apr 1, 2025): Course & Class Module
├── Course Entity & Repository
├── Class Entity & Repository
├── Schedule Management
├── API Endpoints
└── Integration Tests

Week 7-8 (Apr 2 - Apr 15, 2025): Instructor & Payment Module
├── Instructor Management
├── Payment Processing
├── Invoice Management
├── Payment Gateway Integration
└── Tests

Week 9-10 (Apr 16 - Apr 30, 2025): Evaluation & Reporting
├── Assignment & Exam Management
├── Grade Management
├── Reports & Analytics Module
├── Final Integration Tests
└── Bug Fixes
```

#### Phase 3: Testing & QA (3 weeks)
```
Week 1 (May 1 - May 7, 2025)
├── Unit Test Execution & Coverage
├── Integration Test Execution
└── Bug Reporting

Week 2 (May 8 - May 14, 2025)
├── System Testing
├── Performance Testing
├── Security Testing (Penetration Testing)
└── Bug Fixes

Week 3 (May 15 - May 21, 2025)
├── UAT Preparation & Execution
├── Final Bug Fixes
└── Deployment Readiness Review
```

#### Phase 4: Deployment & Go-Live (2 weeks)
```
Week 1 (May 22 - May 28, 2025)
├── Production Database Setup
├── Data Migration from Legacy System
├── User Training Sessions
└── Documentation Finalization

Week 2 (May 29 - Jun 4, 2025)
├── System Deployment to Production
├── Smoke Testing on Production
├── Go-Live Support
└── Performance Monitoring
```

## 3. Resource Planning

### 3.1 Project Team Structure

| Role | Count | Responsibilities |
|------|-------|-----------------|
| **Project Manager** | 1 | Project coordination, timeline management, stakeholder communication |
| **Business Analyst** | 1 | Requirements gathering, documentation, stakeholder liaison |
| **System Architect** | 1 | System design, technical decisions, code reviews |
| **Backend Developers** | 3 | .NET Core API development, database work |
| **Frontend Developers** | 2 | UI/UX implementation, API integration (if needed) |
| **QA Engineer** | 2 | Test planning, execution, bug tracking |
| **Database Administrator** | 1 | Database design, performance tuning, backup strategy |
| **DevOps Engineer** | 1 | Infrastructure, CI/CD, deployment |
| **Tech Lead** | 1 | Technical guidance, best practices, code standards |

**Total Team Size**: 13 people

### 3.2 Skills Required

| Technology | Required Skills | Priority |
|-----------|-----------------|----------|
| .NET Core 8.0 | C#, ASP.NET Core, REST API | High |
| SQL Server | T-SQL, Database Design, Optimization | High |
| Entity Framework Core | ORM, LINQ, Migrations | High |
| Architecture Patterns | Clean Architecture, SOLID, Design Patterns | High |
| Testing | Unit Testing, Integration Testing, Mocking | Medium |
| Git | Version Control, Branching Strategy | Medium |
| Docker & Kubernetes | Containerization, Orchestration | Medium |
| CI/CD | GitHub Actions, Azure DevOps | Medium |

## 4. Budget Estimation

### 4.1 Development Cost Breakdown

| Category | Cost (VND) | Notes |
|----------|-----------|-------|
| **Personnel** | 800,000,000 | 13 team members × 5 months |
| **Infrastructure** | 50,000,000 | Servers, databases, licenses |
| **Software Licenses** | 30,000,000 | .NET, SQL Server, development tools |
| **Training & Documentation** | 20,000,000 | User training, documentation |
| **Testing & QA** | 40,000,000 | Testing tools, UAT activities |
| **Deployment & Support** | 20,000,000 | Go-live support, monitoring |
| **Contingency (10%)** | 95,000,000 | Unexpected costs |
| **TOTAL** | **1,055,000,000** | (~$40,000 USD) |

### 4.2 Ongoing Maintenance Cost (Annual)

| Item | Cost (VND) | Notes |
|------|-----------|-------|
| Server Hosting | 30,000,000 | Cloud infrastructure |
| Software Licenses | 10,000,000 | Renewals |
| Support & Maintenance | 50,000,000 | Bug fixes, updates |
| Training & Enhancement | 20,000,000 | New features, user training |
| **TOTAL ANNUAL** | **110,000,000** | |

## 5. Risk Management

### 5.1 Risk Register

| Risk | Probability | Impact | Mitigation |
|------|------------|--------|-----------|
| **Scope Creep** | High | High | Strict change control, regular stakeholder reviews |
| **Technical Delays** | Medium | High | Experienced team, risk-based scheduling |
| **Data Migration Issues** | Medium | High | Early data audit, migration testing |
| **Resource Unavailability** | Medium | Medium | Cross-training, backup resources |
| **Performance Issues** | Medium | High | Load testing, optimization planning |
| **Security Breaches** | Low | Critical | Security testing, compliance reviews |
| **User Adoption** | Medium | Medium | Training, change management |
| **Infrastructure Failure** | Low | Critical | Redundancy, disaster recovery plan |

### 5.2 Risk Response Strategy

**Scope Creep**
- Change request process with formal approval
- Weekly scope reviews
- Document all requested changes

**Data Migration**
- Conduct data audit 2 months before go-live
- Create validation rules
- Plan rollback procedures

**Performance Issues**
- Database indexing strategy
- Caching implementation (Redis)
- Load testing in staging

## 6. Quality Metrics

### 6.1 Quality Goals

| Metric | Target | Measurement |
|--------|--------|-------------|
| Code Coverage | 80%+ | SonarQube, Codecov |
| Defect Density | < 0.5 per 1000 LOC | Bug tracking system |
| API Response Time | < 500ms | Application Performance Monitoring |
| System Availability | 99.5% | Uptime monitoring |
| Test Pass Rate | 100% | Automated test execution |
| Security Score | A+ | OWASP assessment |

### 6.2 Code Quality Standards

- Follow SOLID principles
- Use design patterns appropriately
- Code review before merge
- Automated formatting & linting
- Documentation requirements

## 7. Communication Plan

### 7.1 Stakeholder Communication

| Stakeholder | Frequency | Method | Content |
|------------|-----------|--------|---------|
| Executive Sponsor | Monthly | Meeting | Progress, risks, budget |
| Project Team | Daily | Standup | Tasks, blockers, updates |
| Business Users | Bi-weekly | Workshop | Features, feedback, training |
| IT Department | Weekly | Meeting | Infrastructure, deployment |

### 7.2 Reporting

- **Weekly Status Report**: Progress, risks, blockers
- **Monthly Report**: Milestone completion, budget, quality metrics
- **Project Health Dashboard**: Real-time view of project status

## 8. Assumptions & Dependencies

### 8.1 Assumptions
- Stakeholders are available for meetings & reviews
- Requirements are stable after Phase 1
- Budget will not be significantly cut
- Team members will be available full-time
- Technology stack will not change during development

### 8.2 Dependencies
- Infrastructure readiness (servers, databases)
- Legacy system data availability
- Third-party integrations (payment gateway, email service)
- Management approval for new policies/procedures

## 9. Success Criteria

✅ All features implemented as per specification  
✅ System passes UAT with < 5 critical bugs  
✅ Performance meets SLAs (< 500ms response)  
✅ Security assessment score: A+  
✅ Code coverage: 80%+  
✅ 95% user satisfaction in post-launch survey  
✅ On-time, within-budget delivery  
✅ Zero data loss during migration

## 10. Change Control Process

### 10.1 Change Request Procedure
1. **Identify**: Describe change requirement
2. **Assess**: Analyze impact on scope, timeline, budget
3. **Approve**: CCB (Change Control Board) review
4. **Implement**: Execute approved change
5. **Verify**: Test & validate change
6. **Close**: Update baselines

### 10.2 CCB Members
- Project Manager (Chair)
- System Architect
- Business Analyst
- Sponsor Representative

---

**Version**: 1.0  
**Last Updated**: January 28, 2026

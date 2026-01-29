using System;
using System.Collections.Generic;

namespace EnglishTrainingCenter.Application.DTOs.StudentAdvanced;

// ============================================================================
// ATTENDANCE RELATED DTOs
// ============================================================================

/// <summary>
/// Represents a single attendance record
/// </summary>
public class AttendanceRecordDto
{
    /// <summary>Record ID</summary>
    public int Id { get; set; }

    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Attendance date</summary>
    public DateTime AttendanceDate { get; set; }

    /// <summary>Attendance status (Present, Absent, Late, Excused)</summary>
    public string Status { get; set; }

    /// <summary>Notes or remarks</summary>
    public string Notes { get; set; }

    /// <summary>Record creation date</summary>
    public DateTime RecordedDate { get; set; }
}

/// <summary>
/// Attendance statistics for a student
/// </summary>
public class AttendanceStatisticsDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student name</summary>
    public string StudentName { get; set; }

    /// <summary>Total classes attended</summary>
    public int TotalPresent { get; set; }

    /// <summary>Total classes absent</summary>
    public int TotalAbsent { get; set; }

    /// <summary>Total late arrivals</summary>
    public int TotalLate { get; set; }

    /// <summary>Total excused absences</summary>
    public int TotalExcused { get; set; }

    /// <summary>Overall attendance percentage</summary>
    public decimal AttendancePercentage { get; set; }

    /// <summary>Attendance trend (improving, stable, declining)</summary>
    public string Trend { get; set; }

    /// <summary>Breakdown by course</summary>
    public List<CourseAttendanceDto> ByCourse { get; set; }

    /// <summary>Risk level based on attendance (Low, Medium, High)</summary>
    public string RiskLevel { get; set; }
}

/// <summary>
/// Attendance by course
/// </summary>
public class CourseAttendanceDto
{
    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Attendance percentage for this course</summary>
    public decimal AttendancePercentage { get; set; }

    /// <summary>Number of classes taken</summary>
    public int ClassesTaken { get; set; }

    /// <summary>Attendance status for this course</summary>
    public string Status { get; set; }
}

/// <summary>
/// Course attendance summary
/// </summary>
public class CourseAttendanceSummaryDto
{
    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Total students enrolled</summary>
    public int TotalEnrolled { get; set; }

    /// <summary>Average attendance percentage</summary>
    public decimal AverageAttendance { get; set; }

    /// <summary>Total classes held</summary>
    public int TotalClassesHeld { get; set; }

    /// <summary>Student attendance details</summary>
    public List<StudentAttendanceDetailDto> StudentDetails { get; set; }

    /// <summary>Attendance distribution (ranges)</summary>
    public Dictionary<string, int> AttendanceDistribution { get; set; }
}

/// <summary>
/// Student attendance detail for course
/// </summary>
public class StudentAttendanceDetailDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student name</summary>
    public string StudentName { get; set; }

    /// <summary>Attendance percentage</summary>
    public decimal AttendancePercentage { get; set; }

    /// <summary>Classes present</summary>
    public int Present { get; set; }

    /// <summary>Classes absent</summary>
    public int Absent { get; set; }

    /// <summary>Late arrivals</summary>
    public int Late { get; set; }

    /// <summary>Attendance status</summary>
    public string Status { get; set; }
}

/// <summary>
/// Bulk attendance record for marking multiple students
/// </summary>
public class BulkAttendanceDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Attendance date</summary>
    public DateTime AttendanceDate { get; set; }

    /// <summary>Attendance status</summary>
    public string Status { get; set; }

    /// <summary>Notes</summary>
    public string Notes { get; set; }
}

/// <summary>
/// Low attendance warning for a student
/// </summary>
public class LowAttendanceWarningDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student name</summary>
    public string StudentName { get; set; }

    /// <summary>Student email</summary>
    public string Email { get; set; }

    /// <summary>Current attendance percentage</summary>
    public decimal CurrentAttendance { get; set; }

    /// <summary>Minimum required attendance</summary>
    public decimal MinimumRequired { get; set; }

    /// <summary>Courses with low attendance</summary>
    public List<string> AffectedCourses { get; set; }

    /// <summary>Days until attendance review</summary>
    public int DaysUntilReview { get; set; }

    /// <summary>Recommended actions</summary>
    public List<string> RecommendedActions { get; set; }
}

// ============================================================================
// PERFORMANCE PREDICTION & ANALYTICS DTOs
// ============================================================================

/// <summary>
/// Performance prediction for a student
/// </summary>
public class PerformancePredictionDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Predicted grade (A, B, C, D, F)</summary>
    public string PredictedGrade { get; set; }

    /// <summary>Predicted GPA</summary>
    public decimal PredictedGPA { get; set; }

    /// <summary>Confidence level (0-1)</summary>
    public decimal ConfidenceLevel { get; set; }

    /// <summary>Probability of passing</summary>
    public decimal ProbabilityOfPassing { get; set; }

    /// <summary>Risk factors identified</summary>
    public List<string> RiskFactors { get; set; }

    /// <summary>Strengths identified</summary>
    public List<string> Strengths { get; set; }

    /// <summary>Prediction date</summary>
    public DateTime PredictionDate { get; set; }

    /// <summary>Basis of prediction (attendance, grades, engagement, etc.)</summary>
    public Dictionary<string, decimal> PredictionBasis { get; set; }
}

/// <summary>
/// Student performance prediction for course
/// </summary>
public class StudentPerformancePredictionDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student name</summary>
    public string StudentName { get; set; }

    /// <summary>Current GPA</summary>
    public decimal CurrentGPA { get; set; }

    /// <summary>Predicted grade</summary>
    public string PredictedGrade { get; set; }

    /// <summary>Confidence percentage</summary>
    public decimal Confidence { get; set; }

    /// <summary>Risk level</summary>
    public string RiskLevel { get; set; }

    /// <summary>Key factors affecting performance</summary>
    public List<string> KeyFactors { get; set; }
}

/// <summary>
/// At-risk student identification
/// </summary>
public class AtRiskStudentDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student name</summary>
    public string StudentName { get; set; }

    /// <summary>Email address</summary>
    public string Email { get; set; }

    /// <summary>Phone number</summary>
    public string Phone { get; set; }

    /// <summary>Current GPA</summary>
    public decimal CurrentGPA { get; set; }

    /// <summary>Risk level (Low, Medium, High, Critical)</summary>
    public string RiskLevel { get; set; }

    /// <summary>Risk score (0-100)</summary>
    public int RiskScore { get; set; }

    /// <summary>Primary risk factors</summary>
    public List<string> RiskFactors { get; set; }

    /// <summary>Courses at risk</summary>
    public List<string> CoursesAtRisk { get; set; }

    /// <summary>Days since last activity</summary>
    public int DaysSinceLastActivity { get; set; }

    /// <summary>Outstanding balance</summary>
    public decimal OutstandingBalance { get; set; }

    /// <summary>Recommended interventions</summary>
    public List<string> RecommendedInterventions { get; set; }

    /// <summary>Last contact date</summary>
    public DateTime? LastContactDate { get; set; }
}

/// <summary>
/// Detailed performance analysis for student
/// </summary>
public class StudentPerformanceAnalysisDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student name</summary>
    public string StudentName { get; set; }

    /// <summary>Current GPA</summary>
    public decimal CurrentGPA { get; set; }

    /// <summary>GPA trend (improving, stable, declining)</summary>
    public string GPATrend { get; set; }

    /// <summary>Attendance percentage</summary>
    public decimal AttendancePercentage { get; set; }

    /// <summary>Engagement score (0-100)</summary>
    public int EngagementScore { get; set; }

    /// <summary>Average study hours per week</summary>
    public decimal AverageStudyHours { get; set; }

    /// <summary>Course-wise performance</summary>
    public List<CoursePerformanceDto> CoursePerformance { get; set; }

    /// <summary>Strengths areas</summary>
    public List<string> Strengths { get; set; }

    /// <summary>Areas for improvement</summary>
    public List<string> Improvements { get; set; }

    /// <summary>Overall assessment</summary>
    public string OverallAssessment { get; set; }

    /// <summary>Recommendations for success</summary>
    public List<string> Recommendations { get; set; }
}

/// <summary>
/// Course performance details
/// </summary>
public class CoursePerformanceDto
{
    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Current grade</summary>
    public string CurrentGrade { get; set; }

    /// <summary>GPA in this course</summary>
    public decimal GPA { get; set; }

    /// <summary>Attendance in this course</summary>
    public decimal Attendance { get; set; }

    /// <summary>Assignment completion percentage</summary>
    public decimal AssignmentCompletion { get; set; }

    /// <summary>Performance trend</summary>
    public string Trend { get; set; }
}

/// <summary>
/// Intervention recommendations for student
/// </summary>
public class InterventionRecommendationsDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student name</summary>
    public string StudentName { get; set; }

    /// <summary>Urgency level</summary>
    public string Urgency { get; set; }

    /// <summary>Recommended interventions</summary>
    public List<InterventionDto> Interventions { get; set; }

    /// <summary>Support resources available</summary>
    public List<SupportResourceDto> SupportResources { get; set; }

    /// <summary>Timeline for interventions</summary>
    public string Timeline { get; set; }

    /// <summary>Success probability with interventions</summary>
    public decimal SuccessProbability { get; set; }
}

/// <summary>
/// Individual intervention recommendation
/// </summary>
public class InterventionDto
{
    /// <summary>Intervention type (tutoring, counseling, etc.)</summary>
    public string Type { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; }

    /// <summary>Priority level</summary>
    public string Priority { get; set; }

    /// <summary>Expected duration</summary>
    public string Duration { get; set; }

    /// <summary>Contact person</summary>
    public string ContactPerson { get; set; }

    /// <summary>Contact details</summary>
    public string ContactDetails { get; set; }
}

/// <summary>
/// Support resource information
/// </summary>
public class SupportResourceDto
{
    /// <summary>Resource name</summary>
    public string Name { get; set; }

    /// <summary>Resource type (tutoring, counseling, etc.)</summary>
    public string Type { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; }

    /// <summary>Availability</summary>
    public string Availability { get; set; }

    /// <summary>Contact information</summary>
    public string ContactInfo { get; set; }
}

/// <summary>
/// Performance benchmark comparison
/// </summary>
public class PerformanceBenchmarkDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student name</summary>
    public string StudentName { get; set; }

    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Student's current grade</summary>
    public decimal StudentGrade { get; set; }

    /// <summary>Class average</summary>
    public decimal ClassAverage { get; set; }

    /// <summary>Department average</summary>
    public decimal DepartmentAverage { get; set; }

    /// <summary>Percentile rank</summary>
    public int PercentileRank { get; set; }

    /// <summary>Position in class</summary>
    public int PositionInClass { get; set; }

    /// <summary>Total students in class</summary>
    public int TotalStudentsInClass { get; set; }

    /// <summary>Performance compared to peers (Above, At, Below)</summary>
    public string ComparisonStatus { get; set; }

    /// <summary>Grade trend</summary>
    public string GradeTrend { get; set; }
}

/// <summary>
/// Student progress tracking
/// </summary>
public class StudentProgressTrackingDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student name</summary>
    public string StudentName { get; set; }

    /// <summary>Overall progress percentage</summary>
    public decimal OverallProgress { get; set; }

    /// <summary>Progress by course</summary>
    public List<CourseProgressDto> CourseProgress { get; set; }

    /// <summary>Milestone achievements</summary>
    public List<MilestoneDto> Milestones { get; set; }

    /// <summary>Historical progress data</summary>
    public List<ProgressPointDto> ProgressHistory { get; set; }

    /// <summary>Projected completion date</summary>
    public DateTime ProjectedCompletion { get; set; }
}

/// <summary>
/// Course progress details
/// </summary>
public class CourseProgressDto
{
    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Completion percentage</summary>
    public decimal CompletionPercentage { get; set; }

    /// <summary>Modules completed</summary>
    public int ModulesCompleted { get; set; }

    /// <summary>Total modules</summary>
    public int TotalModules { get; set; }

    /// <summary>Status (In Progress, At Risk, On Track, Completed)</summary>
    public string Status { get; set; }
}

/// <summary>
/// Milestone achievement
/// </summary>
public class MilestoneDto
{
    /// <summary>Milestone name</summary>
    public string Name { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; }

    /// <summary>Completion date</summary>
    public DateTime? CompletionDate { get; set; }

    /// <summary>Status (Completed, In Progress, Not Started)</summary>
    public string Status { get; set; }
}

/// <summary>
/// Progress data point for history
/// </summary>
public class ProgressPointDto
{
    /// <summary>Date of measurement</summary>
    public DateTime Date { get; set; }

    /// <summary>Progress percentage at this point</summary>
    public decimal ProgressPercentage { get; set; }

    /// <summary>GPA at this point</summary>
    public decimal GPA { get; set; }
}

// ============================================================================
// LEARNING PATHS DTOs
// ============================================================================

/// <summary>
/// Learning path for student
/// </summary>
public class LearningPathDto
{
    /// <summary>Learning path ID</summary>
    public int Id { get; set; }

    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student name</summary>
    public string StudentName { get; set; }

    /// <summary>Path name</summary>
    public string PathName { get; set; }

    /// <summary>Path description</summary>
    public string Description { get; set; }

    /// <summary>Specialization or focus area</summary>
    public string Specialization { get; set; }

    /// <summary>Target completion date</summary>
    public DateTime TargetCompletionDate { get; set; }

    /// <summary>Courses in this path</summary>
    public List<int> CourseIds { get; set; }

    /// <summary>Required milestones</summary>
    public List<string> Milestones { get; set; }

    /// <summary>Skill development areas</summary>
    public List<string> SkillAreas { get; set; }

    /// <summary>Career goals associated</summary>
    public string CareerGoals { get; set; }

    /// <summary>Path status (Active, Completed, On Hold)</summary>
    public string Status { get; set; }

    /// <summary>Created date</summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>Last updated date</summary>
    public DateTime LastUpdated { get; set; }
}

/// <summary>
/// Request to create learning path
/// </summary>
public class CreateLearningPathRequestDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Path name</summary>
    public string PathName { get; set; }

    /// <summary>Path description</summary>
    public string Description { get; set; }

    /// <summary>Specialization focus</summary>
    public string Specialization { get; set; }

    /// <summary>Target completion date</summary>
    public DateTime TargetCompletionDate { get; set; }

    /// <summary>Initial course IDs</summary>
    public List<int> InitialCourseIds { get; set; }

    /// <summary>Career goals</summary>
    public string CareerGoals { get; set; }

    /// <summary>Skill areas to develop</summary>
    public List<string> SkillAreas { get; set; }
}

/// <summary>
/// Request to update learning path
/// </summary>
public class UpdateLearningPathRequestDto
{
    /// <summary>Updated path name</summary>
    public string PathName { get; set; }

    /// <summary>Updated description</summary>
    public string Description { get; set; }

    /// <summary>Updated target date</summary>
    public DateTime? TargetCompletionDate { get; set; }

    /// <summary>Courses to add</summary>
    public List<int> CoursesToAdd { get; set; }

    /// <summary>Courses to remove</summary>
    public List<int> CoursesToRemove { get; set; }

    /// <summary>Updated milestones</summary>
    public List<string> Milestones { get; set; }

    /// <summary>Updated career goals</summary>
    public string CareerGoals { get; set; }
}

/// <summary>
/// Course recommendation
/// </summary>
public class CourseRecommendationDto
{
    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; }

    /// <summary>Recommendation reason</summary>
    public string RecommendationReason { get; set; }

    /// <summary>Relevance score (0-100)</summary>
    public int RelevanceScore { get; set; }

    /// <summary>Difficulty level</summary>
    public string DifficultyLevel { get; set; }

    /// <summary>Prerequisites required</summary>
    public List<string> Prerequisites { get; set; }

    /// <summary>Expected duration (weeks)</summary>
    public int ExpectedDuration { get; set; }

    /// <summary>Skills developed</summary>
    public List<string> SkillsDeveloped { get; set; }
}

/// <summary>
/// Learning path progress tracking
/// </summary>
public class LearningPathProgressDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Learning path ID</summary>
    public int LearningPathId { get; set; }

    /// <summary>Path name</summary>
    public string PathName { get; set; }

    /// <summary>Overall completion percentage</summary>
    public decimal CompletionPercentage { get; set; }

    /// <summary>Completed courses</summary>
    public List<CompletedCourseDto> CompletedCourses { get; set; }

    /// <summary>In progress courses</summary>
    public List<InProgressCourseDto> InProgressCourses { get; set; }

    /// <summary>Remaining courses</summary>
    public List<PendingCourseDto> RemainingCourses { get; set; }

    /// <summary>Completed milestones</summary>
    public List<string> CompletedMilestones { get; set; }

    /// <summary>Pending milestones</summary>
    public List<string> PendingMilestones { get; set; }

    /// <summary>Projected completion date</summary>
    public DateTime ProjectedCompletionDate { get; set; }

    /// <summary>On track status</summary>
    public bool IsOnTrack { get; set; }
}

/// <summary>
/// Completed course details
/// </summary>
public class CompletedCourseDto
{
    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Final grade</summary>
    public string FinalGrade { get; set; }

    /// <summary>Completion date</summary>
    public DateTime CompletionDate { get; set; }
}

/// <summary>
/// In progress course details
/// </summary>
public class InProgressCourseDto
{
    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Current completion percentage</summary>
    public decimal CompletionPercentage { get; set; }

    /// <summary>Current grade</summary>
    public string CurrentGrade { get; set; }

    /// <summary>Expected completion date</summary>
    public DateTime ExpectedCompletionDate { get; set; }
}

/// <summary>
/// Pending course details
/// </summary>
public class PendingCourseDto
{
    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Scheduled start date</summary>
    public DateTime? ScheduledStartDate { get; set; }

    /// <summary>Prerequisites to complete first</summary>
    public List<string> PrerequisitesRequired { get; set; }
}

// ============================================================================
// PREREQUISITE MANAGEMENT DTOs
// ============================================================================

/// <summary>
/// Prerequisite check result
/// </summary>
public class PrerequisiteCheckDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Course ID being checked</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>All prerequisites met</summary>
    public bool AllPrerequisitesMet { get; set; }

    /// <summary>Prerequisites met</summary>
    public List<MetPrerequisiteDto> MetPrerequisites { get; set; }

    /// <summary>Missing prerequisites</summary>
    public List<MissingPrerequisiteDto> MissingPrerequisites { get; set; }

    /// <summary>Can enroll in course</summary>
    public bool CanEnroll { get; set; }

    /// <summary>Alternative paths if prerequisites not met</summary>
    public List<AlternativePathDto> AlternativePaths { get; set; }
}

/// <summary>
/// Met prerequisite
/// </summary>
public class MetPrerequisiteDto
{
    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Grade achieved</summary>
    public string GradeAchieved { get; set; }

    /// <summary>Completion date</summary>
    public DateTime CompletionDate { get; set; }
}

/// <summary>
/// Missing prerequisite
/// </summary>
public class MissingPrerequisiteDto
{
    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Minimum grade required</summary>
    public string MinimumGradeRequired { get; set; }

    /// <summary>When course is offered next</summary>
    public DateTime NextOffering { get; set; }

    /// <summary>Alternative courses that satisfy prerequisite</summary>
    public List<int> AlternativeCourses { get; set; }
}

/// <summary>
/// Alternative path to meet prerequisites
/// </summary>
public class AlternativePathDto
{
    /// <summary>Path description</summary>
    public string Description { get; set; }

    /// <summary>Courses to take in alternative path</summary>
    public List<string> CoursesInPath { get; set; }

    /// <summary>Total duration</summary>
    public string TotalDuration { get; set; }
}

/// <summary>
/// Prerequisite course recommendation
/// </summary>
public class PrerequisiteCourseDto
{
    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Why this is recommended</summary>
    public string Reason { get; set; }

    /// <summary>When course is offered</summary>
    public string Offering { get; set; }

    /// <summary>Duration in weeks</summary>
    public int DurationWeeks { get; set; }

    /// <summary>Skills developed</summary>
    public List<string> SkillsDeveloped { get; set; }
}

// ============================================================================
// STUDY PROGRESS & ENGAGEMENT DTOs
// ============================================================================

/// <summary>
/// Study progress for a course
/// </summary>
public class StudyProgressDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Overall completion percentage</summary>
    public decimal CompletionPercentage { get; set; }

    /// <summary>Modules completed</summary>
    public int ModulesCompleted { get; set; }

    /// <summary>Total modules</summary>
    public int TotalModules { get; set; }

    /// <summary>Assignments completed</summary>
    public int AssignmentsCompleted { get; set; }

    /// <summary>Total assignments</summary>
    public int TotalAssignments { get; set; }

    /// <summary>Average assignment score</summary>
    public decimal AverageAssignmentScore { get; set; }

    /// <summary>Quizzes completed</summary>
    public int QuizzesCompleted { get; set; }

    /// <summary>Average quiz score</summary>
    public decimal AverageQuizScore { get; set; }

    /// <summary>Current grade</summary>
    public string CurrentGrade { get; set; }

    /// <summary>Days remaining in course</summary>
    public int DaysRemaining { get; set; }

    /// <summary>Projected final grade</summary>
    public string ProjectedFinalGrade { get; set; }

    /// <summary>Study pace (On Pace, Ahead, Behind)</summary>
    public string StudyPace { get; set; }
}

/// <summary>
/// Student engagement metrics
/// </summary>
public class StudentEngagementDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Overall engagement score (0-100)</summary>
    public int OverallEngagementScore { get; set; }

    /// <summary>Learning activity engagement</summary>
    public int LearningActivityScore { get; set; }

    /// <summary>Forum participation score</summary>
    public int ForumParticipationScore { get; set; }

    /// <summary>Peer collaboration score</summary>
    public int PeerCollaborationScore { get; set; }

    /// <summary>Assignment submission score</summary>
    public int AssignmentSubmissionScore { get; set; }

    /// <summary>Days since last login</summary>
    public int DaysSinceLastLogin { get; set; }

    /// <summary>Last login date</summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>Resource access frequency</summary>
    public string ResourceAccessFrequency { get; set; }

    /// <summary>Engagement trend</summary>
    public string Trend { get; set; }

    /// <summary>Risk based on engagement</summary>
    public string RiskLevel { get; set; }
}

/// <summary>
/// Study time analytics
/// </summary>
public class StudyTimeAnalyticsDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student name</summary>
    public string StudentName { get; set; }

    /// <summary>Total study hours in period</summary>
    public decimal TotalStudyHours { get; set; }

    /// <summary>Average study hours per week</summary>
    public decimal AverageHoursPerWeek { get; set; }

    /// <summary>Average study hours per day</summary>
    public decimal AverageHoursPerDay { get; set; }

    /// <summary>Most active study day</summary>
    public string MostActiveDayOfWeek { get; set; }

    /// <summary>Most active study time</summary>
    public string MostActiveTimeOfDay { get; set; }

    /// <summary>Study pattern (Consistent, Sporadic, Cramming)</summary>
    public string StudyPattern { get; set; }

    /// <summary>Hours by course</summary>
    public Dictionary<string, decimal> HoursByCourse { get; set; }

    /// <summary>Study time effectiveness rating</summary>
    public decimal EffectivenessRating { get; set; }

    /// <summary>Recommendations for study improvement</summary>
    public List<string> Recommendations { get; set; }

    /// <summary>Daily breakdown</summary>
    public List<DailyStudyDto> DailyBreakdown { get; set; }
}

/// <summary>
/// Daily study record
/// </summary>
public class DailyStudyDto
{
    /// <summary>Date</summary>
    public DateTime Date { get; set; }

    /// <summary>Study hours</summary>
    public decimal StudyHours { get; set; }

    /// <summary>Courses studied</summary>
    public List<string> CoursesTaken { get; set; }
}

/// <summary>
/// Resource utilization
/// </summary>
public class ResourceUtilizationDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student name</summary>
    public string StudentName { get; set; }

    /// <summary>Learning materials accessed</summary>
    public int LearningMaterialsAccessed { get; set; }

    /// <summary>Videos watched</summary>
    public int VideosWatched { get; set; }

    /// <summary>Articles read</summary>
    public int ArticlesRead { get; set; }

    /// <summary>Tutorials used</summary>
    public int TutorialsUsed { get; set; }

    /// <summary>Forum posts read</summary>
    public int ForumPostsRead { get; set; }

    /// <summary>Forum posts written</summary>
    public int ForumPostsWritten { get; set; }

    /// <summary>Most used resources</summary>
    public List<string> MostUsedResources { get; set; }

    /// <summary>Unused resources</summary>
    public List<string> UnusedResources { get; set; }

    /// <summary>Resource utilization percentage</summary>
    public decimal UtilizationPercentage { get; set; }

    /// <summary>Recommendations for better resource use</summary>
    public List<string> Recommendations { get; set; }
}

// ============================================================================
// STUDY GROUP & COLLABORATION DTOs
// ============================================================================

/// <summary>
/// Request to manage study group
/// </summary>
public class ManageStudyGroupRequestDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Study group ID (for joining)</summary>
    public int? GroupId { get; set; }

    /// <summary>Group name (for creating)</summary>
    public string GroupName { get; set; }

    /// <summary>Group description</summary>
    public string Description { get; set; }

    /// <summary>Max members allowed</summary>
    public int? MaxMembers { get; set; }

    /// <summary>Meeting time preference</summary>
    public string MeetingTime { get; set; }

    /// <summary>Action (Create, Join, Leave)</summary>
    public string Action { get; set; }
}

/// <summary>
/// Study group details
/// </summary>
public class StudyGroupDto
{
    /// <summary>Study group ID</summary>
    public int Id { get; set; }

    /// <summary>Group name</summary>
    public string GroupName { get; set; }

    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Group description</summary>
    public string Description { get; set; }

    /// <summary>Group creator ID</summary>
    public int CreatorId { get; set; }

    /// <summary>Creator name</summary>
    public string CreatorName { get; set; }

    /// <summary>Current members</summary>
    public int CurrentMembers { get; set; }

    /// <summary>Max members allowed</summary>
    public int MaxMembers { get; set; }

    /// <summary>Member list</summary>
    public List<StudyGroupMemberDto> Members { get; set; }

    /// <summary>Preferred meeting time</summary>
    public string MeetingTime { get; set; }

    /// <summary>Creation date</summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>Last activity date</summary>
    public DateTime LastActivityDate { get; set; }

    /// <summary>Group status (Active, Inactive, Completed)</summary>
    public string Status { get; set; }
}

/// <summary>
/// Study group member details
/// </summary>
public class StudyGroupMemberDto
{
    /// <summary>Member student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Member name</summary>
    public string StudentName { get; set; }

    /// <summary>Member email</summary>
    public string Email { get; set; }

    /// <summary>Current course grade</summary>
    public string CourseGrade { get; set; }

    /// <summary>Join date</summary>
    public DateTime JoinDate { get; set; }

    /// <summary>Last active date</summary>
    public DateTime LastActiveDate { get; set; }

    /// <summary>Contribution level (Low, Medium, High)</summary>
    public string ContributionLevel { get; set; }
}

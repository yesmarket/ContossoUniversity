namespace ContosoUniversity
{
    public class Query
    {
        /// <summary>
        /// Gets all students.
        /// </summary>
        [UseProjection]
        public IQueryable<Student> GetStudents([Service] SchoolContext schoolContext) =>
             schoolContext.Students;
    }
}
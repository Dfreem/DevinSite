namespace DevinSiteTests;

public class RepoTests
{
    public ISiteRepository Repo { get; set; }
    public RepoTests()
    {
        Repo = new MockRepository();
    }

    [Fact]
    public void TestGetAssignments()
    {
        // Arrange
        List<Assignment> emptyAssignments;
        List<Assignment> assignments;

        // Act
        emptyAssignments = new();
        assignments = Repo.Assignments;

        // Assert
        Assert.Empty(emptyAssignments);
        Assert.NotEmpty(assignments);
        foreach (var item in assignments)
        {
            Assert.NotNull(item);
        }
    }

    [Fact]
    public async Task TestAddAssignmentAsync()
    {
        // Arrange
        Assignment toAdd = new()
        {
            AssignmentId = 1,
            Title = "test",
            Details = "This is a test",
            DueDate = DateTime.Now.AddDays(3)
        };

        // Act
        await Repo.AddAssignmentAsync(toAdd);
        var result = Repo.Assignments.Find(a => a.AssignmentId.Equals(1));

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test", result!.Title);
    }

    [Fact]
    public async Task TestAddAssignmentRangeAsync()
    {
        // Arrange
        List<Assignment> toAdd = new()
        {
            new Assignment
            {
                AssignmentId = 2,
                Title = "test 2",
                Details = "This is test 2",
                DueDate = DateTime.Now.AddDays(4)
            },
            new Assignment
            {
                AssignmentId = 3,
                Title = "test 3",
                Details = "This is test 3",
                DueDate = DateTime.Now.AddDays(5)
            }
        };

        // Act
        await Repo.AddAssignmentRangeAsync(toAdd);
        var result = Repo.Assignments.Find(a => a.AssignmentId.Equals(2));

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test 2", result.Title);
    }

    [Fact]
    public void TestDeleteAllStudentAssignments()
    {
        // Arrange
        var initialCount = Repo.Assignments.Count;

        // Act
        Repo.DeleteAllStudentAssignments();

        // Assert
        Assert.Empty(Repo.Assignments);
        Assert.NotEqual(initialCount, Repo.Assignments.Count);
    }

    [Fact]
    public async Task TestDeleteAssignmentAsync()
    {
        // Arrange
        Assignment toDelete = Repo.Assignments[0];

        // Act
        Repo.DeleteAssignment(toDelete);
        await Task.CompletedTask;

        // Assert
        Assert.DoesNotContain(toDelete, Repo.Assignments);
    }

    [Fact]
    public async Task TestDeleteCourseAsync()
    {
        // Arrange
        Course toDelete = new Course { CourseID = 1, Name = "Course 1" };
        Repo.Courses.Add(toDelete);

        // Act
        Repo.DeleteCourse(toDelete);
        await Task.CompletedTask;

        // Assert
        Assert.DoesNotContain(toDelete, Repo.Courses);
    }

    [Fact]
    public void TestDeleteAssignment()
    {
        // Arrange
        var assignmentToDelete = Repo.Assignments.FirstOrDefault();
        var expectedCount = Repo.Assignments.Count - 1;

        // Act
        Repo.DeleteAssignment(assignmentToDelete!);
        var assignmentsAfterDelete = Repo.Assignments;

        // Assert
        Assert.DoesNotContain(assignmentToDelete, assignmentsAfterDelete);
        Assert.Equal(expectedCount, assignmentsAfterDelete.Count);
    }

    [Fact]
    public void TestAddCourse()
    {
        // Arrange
        var courseToAdd = new Course()
        {
            CourseID = 1,
            Name = "Test Course",
            Instructor = "Test Instructor"
        };

        // Act
        Repo.AddCourseAsync(courseToAdd).Wait();
        var coursesAfterAdd = Repo.Courses;

        // Assert
        Assert.Contains(courseToAdd, coursesAfterAdd);
    }

    [Fact]
    public void TestDeleteCourse()
    {
        // Arrange
        var courseToDelete = new Course()
        {
            Name = "TestCourse",
            Instructor = "instructor"
        };
        Repo.Courses.Add(courseToDelete);
        var expectedCount = Repo.Courses.Count - 1;

        // Act
        Repo.DeleteCourse(courseToDelete);
        var coursesAfterDelete = Repo.Courses;

        // Assert
        Assert.DoesNotContain(courseToDelete, coursesAfterDelete);
        Assert.Equal(expectedCount, coursesAfterDelete.Count);
    }

    //[Fact]  Implement Course Parsing in MoodleWare prior to activating this test.
    //public void TestUpdateCourse()
    //{
    //    // Arrange
    //    var courseToUpdate = Repo.Courses.FirstOrDefault();
    //    var updatedCourse = new Course()
    //    {
    //        CourseID = courseToUpdate.CourseID,
    //        Name = "Updated Course",
    //        Instructor = "Updated Instructor"
    //    };

    //    // Act
    //    Repo.UpdateCourse(updatedCourse);
    //    var coursesAfterUpdate = Repo.Courses;

    //    // Assert
    //    Assert.Contains(updatedCourse, coursesAfterUpdate);
    //    Assert.DoesNotContain(courseToUpdate, coursesAfterUpdate);
    //}

    
}

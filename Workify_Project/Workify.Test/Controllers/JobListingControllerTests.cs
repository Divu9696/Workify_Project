using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Workify.Controllers;
using Workify.DTOs;
using Workify.Services;

namespace Workify.Test.Controllers;

public class JobListingControllerTests
{
    private readonly Mock<IJobListingService> _jobListingServiceMock;
    private readonly Mock<ILogger<JobListingController>> _loggerMock;
    private readonly JobListingController _jobListingController;

    public JobListingControllerTests()
    {
        _jobListingServiceMock = new Mock<IJobListingService>();
        _loggerMock = new Mock<ILogger<JobListingController>>();

        _jobListingController = new JobListingController(_jobListingServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AddJobListing_ShouldReturnSuccess_WhenJobListingIsValid()
    {
        // Arrange
        var jobListingDto = new JobListingDTO { Title = "Software Engineer", EmployerId = 1 };

        // Act
        var result = await _jobListingController.AddJobListing(jobListingDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Job Listing added successfully", result?.Value);
    }

    [Fact]
    public async Task SearchJobs_ShouldReturnFilteredJobs_WhenCriteriaIsMatched()
    {
        // Arrange
        var criteria = new JobSearchCriteriaDTO { Title = "Engineer", Location = "NY" };
        var mockJobs = new List<JobListingResponseDTO>
        {
            new JobListingResponseDTO { Title = "Software Engineer", Location = "NY" }
        };

        // _jobListingServiceMock.Setup(service => service.FilterJobsAsync(criteria)).ReturnsAsync(mockJobs);

        // Act
        var result = await _jobListingController.FilterJobs(criteria) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(mockJobs, result?.Value);
    }
}

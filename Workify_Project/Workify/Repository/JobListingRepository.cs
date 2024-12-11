using System;
using Microsoft.EntityFrameworkCore;
using Workify.Data;
using Workify.DTOs;
using Workify.Models;

namespace Workify.Repository;

public class JobListingRepository : IJobListingRepository
{
    private readonly CareerCrafterDbContext _dbContext;

    public JobListingRepository(CareerCrafterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<JobListing>> GetAllAsync()
    {
        return await _dbContext.JobListings.ToListAsync();
    }

    public async Task<JobListing?> GetByIdAsync(int id)
    {
        return await _dbContext.JobListings.FindAsync(id);
    }

    public async Task AddAsync(JobListing jobListing)
    {
        await _dbContext.JobListings.AddAsync(jobListing);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobListing jobListing)
    {
        _dbContext.JobListings.Update(jobListing);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var jobListing = await GetByIdAsync(id);
        if (jobListing != null)
        {
            _dbContext.JobListings.Remove(jobListing);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<JobListing>> SearchJobsAsync(JobSearchCriteriaDTO criteria)
    {
        return await _dbContext.JobListings
            .Where(j => j.Title.Contains(criteria.Title) &&
                        j.Location == criteria.Location &&
                        j.Salary >= criteria.MinSalary &&
                        j.JobType == criteria.JobType)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobListing>> GetJobListingsByEmployerIdAsync(int employerId)
    {
        return await _dbContext.JobListings
            .Where(j => j.EmployerId == employerId)
            .ToListAsync();
    }
    public IQueryable<JobListing> Query()
    {
        return _dbContext.JobListings.AsQueryable();
    }
}

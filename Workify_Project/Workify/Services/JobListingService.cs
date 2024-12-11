using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Workify.DTOs;
using Workify.Models;
using Workify.Repository;

namespace Workify.Services;

public class JobListingService : IJobListingService
{
    private readonly IJobListingRepository _jobListingRepository;
    private readonly IMapper _mapper;

    public JobListingService(IJobListingRepository jobListingRepository, IMapper mapper)
    {
        _jobListingRepository = jobListingRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<JobListingResponseDTO>> SearchJobsAsync(JobSearchCriteriaDTO criteria)
    {
        var jobs = await _jobListingRepository.SearchJobsAsync(criteria);
        return _mapper.Map<IEnumerable<JobListingResponseDTO>>(jobs);
    }

    public async Task<IEnumerable<JobListingResponseDTO>> GetJobListingsByEmployerIdAsync(int employerId)
    {
        var jobs = await _jobListingRepository.GetJobListingsByEmployerIdAsync(employerId);
        return _mapper.Map<IEnumerable<JobListingResponseDTO>>(jobs);
    }

    public async Task<JobListingResponseDTO?> GetJobListingByIdAsync(int id)
    {
        var job = await _jobListingRepository.GetByIdAsync(id);
        return job == null ? null : _mapper.Map<JobListingResponseDTO>(job);
    }

    public async Task AddJobListingAsync(JobListingDTO jobListingDto)
    {
        var jobListing = _mapper.Map<JobListing>(jobListingDto);
        await _jobListingRepository.AddAsync(jobListing);
    }

    public async Task UpdateJobListingAsync(int id, JobListingDTO jobListingDto)
    {
        var jobListing = await _jobListingRepository.GetByIdAsync(id);
        if (jobListing == null) throw new KeyNotFoundException("Job listing not found");

        _mapper.Map(jobListingDto, jobListing);
        await _jobListingRepository.UpdateAsync(jobListing);
    }

    public async Task DeleteJobListingAsync(int id)
    {
        await _jobListingRepository.DeleteAsync(id);
    }
    

    public async Task<IEnumerable<JobListingDTO>> FilterJobsAsync(JobSearchCriteriaDTO criteria)
{
    var jobsQuery = _jobListingRepository.Query();

    if (!string.IsNullOrEmpty(criteria.Skills))
    {
        var skillSet = criteria.Skills.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(skill => skill.Trim());

        jobsQuery = jobsQuery.Where(jl => 
            skillSet.Any(skill => jl.Qualifications.Contains(skill)));
    }

    // if (!string.IsNullOrEmpty(criteria.Location))
    // {
    //     jobsQuery = jobsQuery.Where(jl => jl.Location.Contains(criteria.Location));
    // }

    if (criteria.MinSalary > 0)
    {
        jobsQuery = jobsQuery.Where(jl => jl.Salary >= criteria.MinSalary);
    }

    if (!string.IsNullOrEmpty(criteria.JobType))
    {
        jobsQuery = jobsQuery.Where(jl => jl.JobType == criteria.JobType);
    }

    var filteredJobs = await jobsQuery.ToListAsync();
    return _mapper.Map<IEnumerable<JobListingDTO>>(filteredJobs);
}
    
}


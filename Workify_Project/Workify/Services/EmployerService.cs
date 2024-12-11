using System;
using AutoMapper;
using Workify.DTOs;
using Workify.Models;
using Workify.Repository;

namespace Workify.Services;

public class EmployerService : IEmployerService
{
    private readonly IEmployerRepository _employerRepository;
    private readonly IMapper _mapper;

    public EmployerService(IEmployerRepository employerRepository, IMapper mapper)
    {
        _employerRepository = employerRepository;
        _mapper = mapper;
    }

    public async Task<EmployerResponseDTO?> GetEmployerByIdAsync(int id)
    {
        var employer = await _employerRepository.GetByIdAsync(id);
        return employer == null ? null : _mapper.Map<EmployerResponseDTO>(employer);
    }

    public async Task<EmployerResponseDTO?> GetEmployerByUserIdAsync(int userId)
    {
        var employer = await _employerRepository.GetByUserIdAsync(userId);
        return employer == null ? null : _mapper.Map<EmployerResponseDTO>(employer);
    }
    public async Task<IEnumerable<EmployerDTO>> GetAllEmployersAsync()
    {
        // Retrieve all employers from the repository
        var employers = await _employerRepository.GetAllAsync();

        // Map entities to DTOs
        var employerDTOs = _mapper.Map<IEnumerable<EmployerDTO>>(employers);

        return employerDTOs;
    }

    public async Task AddEmployerAsync(EmployerDTO employerDto)
    {
        var employer = _mapper.Map<Employer>(employerDto);
        await _employerRepository.AddAsync(employer);
    }

    public async Task UpdateEmployerAsync(int id, EmployerDTO employerDto)
    {
        var employer = await _employerRepository.GetByIdAsync(id);
        if (employer == null) throw new KeyNotFoundException("Employer not found");

        _mapper.Map(employerDto, employer);
        await _employerRepository.UpdateAsync(employer);
    }

    public async Task DeleteEmployerAsync(int id)
    {
        await _employerRepository.DeleteAsync(id);
    }
}

using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Common;
using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Business.Mappings;

public class CommonMappingProfile : Profile
{
    public CommonMappingProfile()
    {
        // Instrument -> InstrumentDto
        CreateMap<Instrument, InstrumentDto>();

        // Classroom -> ClassroomDto
        CreateMap<Classroom, ClassroomDto>();
    }
}
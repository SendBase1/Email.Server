using AutoMapper;
using Email.Server.DTOs.Responses;
using Email.Server.Models;

namespace Email.Server.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Domain mappings
        CreateMap<Domains, DomainResponse>();

        // DNS Record mappings
        CreateMap<DomainDnsRecords, DnsRecordResponse>();

        // Message mappings
        CreateMap<Messages, MessageResponse>();

        // Message Recipient mappings
        CreateMap<MessageRecipients, MessageRecipientResponse>();

        // Message Event mappings
        CreateMap<MessageEvents, MessageEventResponse>();

        // Message Tags mappings (Dictionary)
        CreateMap<Messages, MessageResponse>()
            .ForMember(dest => dest.Tags, opt => opt.Ignore()); // Will be handled manually in service
    }
}

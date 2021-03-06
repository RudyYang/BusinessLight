﻿using AutoMapper;
using BusinessLight.PhoneBook.Domain;
using BusinessLight.PhoneBook.Dto;

namespace BusinessLight.PhoneBook.Mapping
{
    public class ContactToContactDto : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Contact, ContactDto>()
                .ForMember(x => x.ContactInfosCount, opt => opt.MapFrom(y => y.ContactInfos.Count));
        }
    }
}
using Mapster;
using Microsoft.EntityFrameworkCore;
using TaskManagment.Core.Abstractions.Const;
using TaskManagment.Core.Entities;
using TaskManagment.Core.Service;
using TaskManagment.Core.Specification.EntitySpecification;
using TaskManagment.Core.UnitOfWork;
using TaskManagment.Core.ViewModel;
using TaskManagment.ServiceAndFactory.Service;

namespace TaskManagment.ServiceAndFactore.Service
{
    public class EventService(IUnitOfWork unitOfWork) : IEventService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        //private readonly IEventRegistrationService _eventRegistrationService = eventRegistrationService;
        //public async Task<IEnumerable<EventViewModel>> GetAllEventsAsync(EventSpecification sp)
        //{
        //    var events = await _unitOfWork.Repositories<Event>().GetAllWithSpecificationAsync(sp);
        //    return events.Adapt<IEnumerable<EventViewModel>>();
        //}

        //public async Task<EventViewModel?> GetEventByIdAsync(EventSpecification sp)
        //{
        //    var eventEntity = await _unitOfWork.Repositories<Event>().GetByIdWithSpecificationAsync(sp);
        //    return eventEntity?.Adapt<EventViewModel>();
        //}
        //public async Task<Event> GetEventEntityByIdAsync(EventSpecification sp)
        //{
        //    return await _unitOfWork.Repositories<Event>().GetByIdWithSpecificationAsync(sp);
        //}

        //public async Task<Event> AddEventAsync(EventFormViewModel model, EventSpecification sp)
        //{
        //    await _unitOfWork.BeginTransactionAsync();
        //    try
        //    {
        //        if (await _unitOfWork.Repositories<Event>().GetByIdWithSpecificationAsync(sp) is not null)
        //            throw new Exception("Event already exists");

        //        var newEvent = model.Adapt<Event>();
        //        newEvent = await _unitOfWork.Repositories<Event>().AddAsync(newEvent);
        //        await _unitOfWork.CompleteAsync();
        //        var eventTemplateMaps = model.SelectTemplatesId.Select(templateId => new EventTemplateMap
        //        {
        //            EventId = newEvent.Id,
        //            TemplateId = templateId
        //        }).ToList();

        //        await _unitOfWork.Repositories<EventTemplateMap>().AddRangeAsync(eventTemplateMaps);
        //        await _unitOfWork.CompleteAsync();
        //        await _unitOfWork.CommitTransactionAsync();
        //        return newEvent;
        //    }
        //    catch
        //    {
        //        await _unitOfWork.RollbackTransactionAsync();
        //        throw;
        //    }
        //}

        //public async Task<Event> UpdateEventAsync(EventFormViewModel model, EventSpecification sp)
        //{
        //    // جلب الحدث قبل بدء المعاملة
        //    var eventEntity = await _unitOfWork.Repositories<Event>().GetByIdWithSpecificationAsync(sp);
        //    if (eventEntity is null)
        //        throw new Exception("Event not found");

        //    // البدء في المعاملة
        //    await _unitOfWork.BeginTransactionAsync();
        //    try
        //    {
        //        var config = new TypeAdapterConfig();
        //        config.NewConfig<EventFormViewModel, Event>()
        //            .Ignore(dest => dest.Id)
        //            .Ignore(dest => dest.CreatedOn)
        //            .Ignore(dest => dest.EventTemplateMaps);

        //        model.Adapt(eventEntity, config);

        //        _unitOfWork.Repositories<Event>().Update(eventEntity);

        //        var existTemplates = eventEntity.EventTemplateMaps.Select(x => x.TemplateId).ToList();
        //        var removedTemplates = existTemplates.Except(model.SelectTemplatesId).ToList();
        //        var addedTemplates = model.SelectTemplatesId.Except(existTemplates).ToList();

        //        if (removedTemplates.Any())
        //        {
        //            foreach (var templateId in removedTemplates)
        //            {
        //                var reminderEmailList = await _unitOfWork.Repositories<RemainderEmail>().GetAllWithSpecificationAsync(new RemainderEmailSpecification(eventId: model.Id, templateId: templateId));
        //                if (reminderEmailList.Any())
        //                    _unitOfWork.Repositories<RemainderEmail>().DeleteRange(reminderEmailList.ToList()!);
        //            }
        //            if (eventEntity.EventTemplateMaps.Where(x => removedTemplates.Contains(x.TemplateId)).ToList().Any())
        //            {
        //                foreach (var eventTemplateMap in eventEntity.EventTemplateMaps.Where(x => removedTemplates.Contains(x.TemplateId)).ToList())
        //                {
        //                    switch (eventTemplateMap.Template.TemplateType)
        //                    {
        //                        case TemplateType.Location:
        //                            var locationTemplates = await _eventRegistrationService.GetAllLocationTemplateRegister(new LocationTemplateSpecification(id: null, eventTemplateMap.Id)).ToListAsync();
        //                            if (locationTemplates.Any())
        //                            {
        //                                _unitOfWork.Repositories<LocationTemplate>().DeleteRange(
        //                                    locationTemplates
        //                                );
        //                            }
        //                            break;
        //                        case TemplateType.Visitor:
        //                            var registerTemplates = await _eventRegistrationService.GetAllRegisterTemplates(new RegisterTemplateSpecification(id: null, eventTemplateMap.Id)).ToListAsync();
        //                            if (registerTemplates.Any())
        //                            {
        //                                var registerTemplateInterestingMaps = registerTemplates?.SelectMany(x => x.registerTemplateInterestingMaps)?.ToList();
        //                                if (registerTemplateInterestingMaps.Any())
        //                                    _unitOfWork.Repositories<RegisterTemplateInterestingMap>().DeleteRange(
        //                                        registerTemplateInterestingMaps
        //                                    );
        //                                _unitOfWork.Repositories<RegisterTemplate>().DeleteRange(
        //                                    registerTemplates
        //                                );
        //                            }
        //                            break;

        //                        case TemplateType.Sponsor:
        //                            var sponsorTemplates = await _eventRegistrationService.GetAllSponsorTemplateRegister(new SponsorTemplateSpecification(id: null, eventTemplateMap.Id)).ToListAsync();
        //                            if (sponsorTemplates.Any())
        //                            {
        //                                var SponsorTemplateSponsorTypeMaps = sponsorTemplates?.SelectMany(x => x.SponsorTemplateSponsorTypeMaps)?.ToList();
        //                                if (SponsorTemplateSponsorTypeMaps.Any())
        //                                    _unitOfWork.Repositories<SponsorTemplateSponsorTypeMap>().DeleteRange(
        //                                        SponsorTemplateSponsorTypeMaps
        //                                    );
        //                                _unitOfWork.Repositories<SponsorTemplate>().DeleteRange(
        //                                    sponsorTemplates
        //                                );
        //                            }
        //                            break;
        //                    }
        //                }
        //                _unitOfWork.Repositories<EventTemplateMap>().DeleteRange(
        //                eventEntity.EventTemplateMaps.Where(x => removedTemplates.Contains(x.TemplateId)).ToList());
        //            }
        //        }

        //        var newTemplateMaps = addedTemplates.Select(templateId => new EventTemplateMap
        //        {
        //            EventId = eventEntity.Id,
        //            TemplateId = templateId
        //        }).ToList();

        //        if (newTemplateMaps.Any())
        //        {
        //            await _unitOfWork.Repositories<EventTemplateMap>().AddRangeAsync(newTemplateMaps);
        //        }

        //        await _unitOfWork.CompleteAsync();
        //        await _unitOfWork.CommitTransactionAsync();
        //        return eventEntity;
        //    }
        //    catch (Exception ex)
        //    {
        //        await _unitOfWork.RollbackTransactionAsync();
        //        throw;
        //    }
        //}

        //public async Task DeleteEventAsync(EventSpecification sp)
        //{
        //    await _unitOfWork.BeginTransactionAsync();
        //    try
        //    {
        //        var eventEntity = await _unitOfWork.Repositories<Event>().GetByIdWithSpecificationAsync(sp);
        //        if (eventEntity is null)
        //            throw new Exception("Event not found");
        //        var remainderEmails = eventEntity.EventTemplateMaps.SelectMany(x => x.RemainderEmails).ToList();
        //        if (remainderEmails.Any())
        //        {
        //            _unitOfWork.Repositories<RemainderEmail>().DeleteRange(
        //                remainderEmails
        //            );
        //        }
        //        var eventTemplateMaps = eventEntity.EventTemplateMaps.ToList();
        //        if (eventTemplateMaps.Any())
        //        {
        //            foreach (var eventTemplateMap in eventTemplateMaps)
        //            {
        //                switch(eventTemplateMap.Template.TemplateType)
        //                {
        //                    case TemplateType.Location:
        //                        var locationTemplates = await _eventRegistrationService.GetAllLocationTemplateRegister(new LocationTemplateSpecification(id: null, eventTemplateMap.Id)).ToListAsync();
        //                        if(locationTemplates.Any())
        //                        {
        //                            _unitOfWork.Repositories<LocationTemplate>().DeleteRange(
        //                                locationTemplates
        //                            );
        //                        }
        //                        break;
        //                    case TemplateType.Visitor:
        //                        var registerTemplates = await _eventRegistrationService.GetAllRegisterTemplates(new RegisterTemplateSpecification(id: null, eventTemplateMap.Id)).ToListAsync();
        //                        if (registerTemplates.Any())
        //                        {
        //                            var registerTemplateInterestingMaps = registerTemplates?.SelectMany(x => x.registerTemplateInterestingMaps)?.ToList();
        //                            if (registerTemplateInterestingMaps.Any())
        //                                    _unitOfWork.Repositories<RegisterTemplateInterestingMap>().DeleteRange(
        //                                        registerTemplateInterestingMaps
        //                                    );
        //                            _unitOfWork.Repositories<RegisterTemplate>().DeleteRange(
        //                                registerTemplates
        //                            );
        //                        }
        //                        break;

        //                    case TemplateType.Sponsor:
        //                        var sponsorTemplates = await _eventRegistrationService.GetAllSponsorTemplateRegister(new SponsorTemplateSpecification(id: null, eventTemplateMap.Id)).ToListAsync();
        //                        if(sponsorTemplates.Any())
        //                        {
        //                            var SponsorTemplateSponsorTypeMaps = sponsorTemplates?.SelectMany(x => x.SponsorTemplateSponsorTypeMaps)?.ToList();
        //                            if (SponsorTemplateSponsorTypeMaps.Any())
        //                                _unitOfWork.Repositories<SponsorTemplateSponsorTypeMap>().DeleteRange(
        //                                    SponsorTemplateSponsorTypeMaps
        //                                );
        //                            _unitOfWork.Repositories<SponsorTemplate>().DeleteRange(
        //                                sponsorTemplates
        //                            );
        //                        }
        //                        break;
        //                }
        //            }
        //            _unitOfWork.Repositories<EventTemplateMap>().DeleteRange(
        //                eventTemplateMaps
        //            );
        //        }
        //        _unitOfWork.Repositories<Event>().Delete(eventEntity);
        //        await _unitOfWork.CompleteAsync();
        //        await _unitOfWork.CommitTransactionAsync();
        //    }
        //    catch
        //    {
        //        await _unitOfWork.RollbackTransactionAsync();
        //        throw;
        //    }
        //}
        //public async Task<EventTemplateMap?> GetEventIdByEventTemplateMapIdAsync(EventTemplateMapSpecification sp)
        //{
        //    return await _unitOfWork.Repositories<EventTemplateMap>().GetByIdWithSpecificationAsync(sp);
        //}
    }
}
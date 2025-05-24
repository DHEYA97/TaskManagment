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
        public async Task<IEnumerable<EventViewModel>> GetAllEventsAsync(EventSpecification sp)
        {
            var events = await _unitOfWork.Repositories<Event>().GetAllWithSpecificationAsync(sp);
            return events.Adapt<IEnumerable<EventViewModel>>();
        }

        public async Task<EventViewModel?> GetEventByIdAsync(EventSpecification sp)
        {
            var eventEntity = await _unitOfWork.Repositories<Event>().GetByIdWithSpecificationAsync(sp);
            return eventEntity?.Adapt<EventViewModel>();
        }
        public async Task<Event> GetEventEntityByIdAsync(EventSpecification sp)
        {
            return await _unitOfWork.Repositories<Event>().GetByIdWithSpecificationAsync(sp);
        }

        public async Task<Event> AddEventAsync(EventFormViewModel model, EventSpecification sp)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (await _unitOfWork.Repositories<Event>().GetByIdWithSpecificationAsync(sp) is not null)
                    throw new Exception("Event already exists");

                var newEvent = model.Adapt<Event>();
                newEvent = await _unitOfWork.Repositories<Event>().AddAsync(newEvent);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return newEvent;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<Event> UpdateEventAsync(EventFormViewModel model, EventSpecification sp)
        {
            var eventEntity = await _unitOfWork.Repositories<Event>().GetByIdWithSpecificationAsync(sp);
            if (eventEntity is null)
                throw new Exception("Event not found");
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var config = new TypeAdapterConfig();
                config.NewConfig<EventFormViewModel, Event>()
                    .Ignore(dest => dest.Id)
                    .Ignore(dest => dest.CreatedOn);

                model.Adapt(eventEntity, config);

                _unitOfWork.Repositories<Event>().Update(eventEntity);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return eventEntity;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteEventAsync(EventSpecification sp)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var eventEntity = await _unitOfWork.Repositories<Event>().GetByIdWithSpecificationAsync(sp);
                if (eventEntity is null)
                    throw new Exception("Event not found");
                _unitOfWork.Repositories<Event>().Delete(eventEntity);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

    }
}
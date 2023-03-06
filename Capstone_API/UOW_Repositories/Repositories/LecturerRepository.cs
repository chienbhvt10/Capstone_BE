using Capstone_API.Enum;
using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.Repositories
{

    public class LecturerRepository : GenericRepository<Lecturer>, ILecturerRepository
    {
        CapstoneDataContext _context;

        public LecturerRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }

        #region Deletes

        //// Delete Options

        /// <summary>
        /// This method use to delete and entity by id
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="isHardDeleted"></param>
        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _context.Lecturers.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(Lecturer)}");

            if (isHardDeleted == false)
            {
                entity.ExistStatus = Status.Deleted.ToString();
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.Lecturers.Remove(entity);
        }

        /// <summary>
        /// This method use to delete an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isHardDeleted"></param>
        public virtual void Delete(Lecturer entity, bool isHardDeleted = false)
        {
            var entityExist = _context.Lecturers.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(Lecturer)}");

            if (isHardDeleted == false)
            {
                entity.ExistStatus = Status.Deleted.ToString();
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.Lecturers.Remove(entity);
        }

        /// <summary>
        /// This method use to delete an array entities
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <param name="keyValues"></param>
        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _context.Lecturers.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(Lecturer)}");

            if (isHardDeleted == false)
            {
                entitiesExist.ExistStatus = Status.Deleted.ToString();
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _context.Lecturers.Remove(entitiesExist);
        }

        /// <summary>
        /// This method use to delete an array by entity id
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <param name="entity"></param>
        public virtual async Task DeleteAsync(Lecturer entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _context.Lecturers.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(Lecturer)}");

            if (isHardDeleted == false)
            {
                entitiesExist.ExistStatus = Status.Deleted.ToString();
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.Lecturers.Remove(entitiesExist);
        }

        /// <summary>
        /// This method use to delete a params objects key value
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _context.Lecturers.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(Lecturer)}");

            if (isHardDeleted == false)
            {
                entitiesExist.ExistStatus = Status.Deleted.ToString();
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.Lecturers.Remove(entitiesExist);
        }

        /// <summary>
        /// This method use to delete entity by some condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="isHardDeleted"></param>
        public virtual void DeleteByCondition(Func<Lecturer, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.Lecturers.Where(condition);
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        /// <summary>
        /// This method use to delete entity by some condition by async
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="isHardDeleted"></param>
        /// <returns></returns>
        public virtual async Task DeleteByConditionAsync(Func<Lecturer, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.Lecturers.Where(condition);
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }

        #endregion
    }
}

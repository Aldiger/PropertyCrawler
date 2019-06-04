using Microsoft.EntityFrameworkCore;
using RightMove.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMove.Data.Repositories
{
    public interface IPostalCodeRepository
    {
        IQueryable<PostalCodeModel> GetAllPostalCodesAsync();

        Task CreatePostalCodeAsync(PostalCodeModel model);

        Task UpdatePostalCodeAsync(PostalCodeModel model);

        Task DeletePostalCodeAsync(int id);

        Task<PostalCodeModel> GetById(int? id);

    }


    public class PostalCodeRepository : IPostalCodeRepository
    {
        AppContext _context;

        public PostalCodeRepository(AppContext context)
        {
            _context = context;
        }


        public async Task<PostalCodeModel> GetById(int? id)
        {
            return await _context.PostalCodes.Where(a => a.Id == id.Value).Select(x => new PostalCodeModel
            {
                Id = x.Id,
                Active = x.Active,
                Code = x.Code,
                DateAdded = x.DateAdded,
                DateModified = x.DateModified,
                OpCode = x.OpCode
            }).FirstOrDefaultAsync();
        }

        public IQueryable<PostalCodeModel> GetAllPostalCodesAsync()
        {
            return _context.PostalCodes.Where(x=> x.Active==true).Select(x => new PostalCodeModel
            {
                Id = x.Id,
                Active = x.Active,
                Code = x.Code,
                DateAdded = x.DateAdded,
                DateModified = x.DateModified,
                OpCode = x.OpCode
            }).AsQueryable();
        }

        public async Task CreatePostalCodeAsync(PostalCodeModel model)
        {
            _context.PostalCodes.Add(new PostalCode
            {
                Code = model.Code,
                OpCode = model.OpCode,
                Urls = null,
                DateAdded = DateTime.UtcNow,
                Active = true
            });


            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostalCodeAsync(PostalCodeModel model)
        {
            var dbModel = await _context.PostalCodes.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            dbModel.DateModified = DateTime.UtcNow;
            dbModel.Code = model.Code;
            dbModel.DateAdded = model.DateAdded;
            dbModel.Active = true;
            dbModel.OpCode = model.OpCode;

            _context.Entry(dbModel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostalCodeAsync(int id)
        {
            var model = await _context.PostalCodes.Where(x => x.Id == id).FirstOrDefaultAsync();

            model.Active = false;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


    }
}

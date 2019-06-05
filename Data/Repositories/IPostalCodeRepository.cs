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
        IQueryable<PostalCodeModel> GetAllPostalCodesAsync(string sort);

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
            return await _context.PostalCodes.Include(z => z.Urls).Where(a => a.Id == id.Value).Select(x => new PostalCodeModel
            {
                Id = x.Id,
                Active = x.Active,
                Code = x.Code,
                DateAdded = x.DateAdded,
                DateModified = x.DateModified,
                OpCode = x.OpCode,
                Properties = x.Urls.Count
            }).FirstOrDefaultAsync();
        }

        public IQueryable<PostalCodeModel> GetAllPostalCodesAsync( string sort)
        {
            if (sort == "dcreated")
            {
                return _context.PostalCodes.Where(x => x.Active == true).Include(z=> z.Urls).Select(x => new PostalCodeModel
                {
                    Id = x.Id,
                    Active = x.Active,
                    Code = x.Code,
                    DateAdded = x.DateAdded,
                    DateModified = x.DateModified,
                    OpCode = x.OpCode,
                    Properties = x.Urls.Count

                }).OrderBy(y => y.DateAdded).AsQueryable();
            }
            else if (sort == "dmodified")
            {
                return _context.PostalCodes.Where(x => x.Active == true).Include(z => z.Urls).Select(x => new PostalCodeModel
                {
                    Id = x.Id,
                    Active = x.Active,
                    Code = x.Code,
                    DateAdded = x.DateAdded,
                    DateModified = x.DateModified,
                    OpCode = x.OpCode,
                    Properties = x.Urls.Count
                }).OrderBy(y => y.DateModified).AsQueryable();
            }
            else if (sort == "code")
            {
                return _context.PostalCodes.Where(x => x.Active == true).Include(z => z.Urls).Select(x => new PostalCodeModel
                {
                    Id = x.Id,
                    Active = x.Active,
                    Code = x.Code,
                    DateAdded = x.DateAdded,
                    DateModified = x.DateModified,
                    OpCode = x.OpCode,
                    Properties = x.Urls.Count
                }).OrderBy(y => y.Code).AsQueryable();
            }
            else if (sort == "mostProp")
            {
                return _context.PostalCodes.Where(x => x.Active == true).Include(z => z.Urls).Select(x => new PostalCodeModel
                {
                    Id = x.Id,
                    Active = x.Active,
                    Code = x.Code,
                    DateAdded = x.DateAdded,
                    DateModified = x.DateModified,
                    OpCode = x.OpCode,
                    Properties = x.Urls.Count
                }).OrderByDescending(y => y.Properties).AsQueryable();
            }
            else
                return _context.PostalCodes.Where(x => x.Active == true).Include(z => z.Urls).Select(x => new PostalCodeModel
                {
                    Id = x.Id,
                    Active = x.Active,
                    Code = x.Code,
                    DateAdded = x.DateAdded,
                    DateModified = x.DateModified,
                    OpCode = x.OpCode,
                    Properties = x.Urls.Count
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

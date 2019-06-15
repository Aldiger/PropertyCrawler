using Microsoft.EntityFrameworkCore;
using PropertyCrawler.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCrawler.Data.Repositories
{
    public interface IProcessRepository
    {

    }


    public class ProcessRepository : IProcessRepository
    {
        //AppContext _context;

        //public PostalCodeRepository(AppContext context)
        //{
        //    _context = context;
        //}


        //public async Task<PostalCodeModel> GetById(int? id)
        //{
        //    return await _context.PostalCodes.Include(z => z.Urls).Where(a => a.Id == id.Value).Select(x => new PostalCodeModel
        //    {
        //        Id = x.Id,
        //        Active = x.Active,
        //        Code = x.Code,
        //        DateAdded = x.DateAdded,
        //        DateModified = x.DateModified,
        //        OpCode = x.OpCode,
        //        Properties = x.Urls.Count
        //    }).FirstOrDefaultAsync();
        //}

        //public  async Task<List<string>> AllPostalCodesSelect()
        //{
        //    return await  _context.PostalCodes.Select(x => x.Code).ToListAsync();

        //}


        //public IQueryable<PostalCodeModel> GetAllPostalCodesAsync(string sort, List<string> postal_code)
        //{

        //    if (postal_code.Count != 0)
        //    {
        //        var list = _context.PostalCodes.Where(x => x.Active == true && postal_code.Contains(x.Code)).Include(z => z.Urls).Select(x => new PostalCodeModel
        //        {
        //            Id = x.Id,
        //            Active = x.Active,
        //            Code = x.Code,
        //            DateAdded = x.DateAdded,
        //            DateModified = x.DateModified,
        //            OpCode = x.OpCode,
        //            Properties = x.Urls.Count

        //        }).AsQueryable();
        //        if (sort == "dcreated")
        //        {
        //            return list.OrderBy(y => y.DateAdded);
        //        }
        //        else if (sort == "dmodified")
        //        {
        //            return list.OrderBy(y => y.DateModified);
        //        }
        //        else if (sort == "code")
        //        {
        //            return list.OrderBy(y => y.Code);
        //        }
        //        else if (sort == "mostProp")
        //        {
        //            return list.OrderByDescending(y => y.Properties);
        //        }
        //        else
        //            return list;
        //    }
        //    else
        //    {
        //        var list = _context.PostalCodes.Where(x => x.Active == true).Include(z => z.Urls).Select(x => new PostalCodeModel
        //        {
        //            Id = x.Id,
        //            Active = x.Active,
        //            Code = x.Code,
        //            DateAdded = x.DateAdded,
        //            DateModified = x.DateModified,
        //            OpCode = x.OpCode,
        //            Properties = x.Urls.Count

        //        }).AsQueryable();
        //        if (sort == "dcreated")
        //        {
        //            return list.OrderBy(y => y.DateAdded);
        //        }
        //        else if (sort == "dmodified")
        //        {
        //            return list.OrderBy(y => y.DateModified);
        //        }
        //        else if (sort == "code")
        //        {
        //            return list.OrderBy(y => y.Code);
        //        }
        //        else if (sort == "mostProp")
        //        {
        //            return list.OrderByDescending(y => y.Properties);
        //        }
        //        else
        //            return list;
        //    }

           

            

        //}

        //public async Task CreatePostalCodeAsync(PostalCodeModel model)
        //{
        //    _context.PostalCodes.Add(new PostalCode
        //    {
        //        Code = model.Code,
        //        OpCode = model.OpCode,
        //        Urls = null,
        //        DateAdded = DateTime.UtcNow,
        //        Active = true
        //    });


        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdatePostalCodeAsync(PostalCodeModel model)
        //{
        //    var dbModel = await _context.PostalCodes.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

        //    dbModel.DateModified = DateTime.UtcNow;
        //    dbModel.Code = model.Code;
        //    dbModel.DateAdded = model.DateAdded;
        //    dbModel.Active = true;
        //    dbModel.OpCode = model.OpCode;

        //    _context.Entry(dbModel).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeletePostalCodeAsync(int id)
        //{
        //    var model = await _context.PostalCodes.Where(x => x.Id == id).FirstOrDefaultAsync();

        //    model.Active = false;
        //    _context.Entry(model).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}


    }
}

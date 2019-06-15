using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMove.Data.Repositories
{
    public interface IProcessRepository
    {
        //Task GetAllProcesses();

        //Task GetProcessById();

        //Task StartProcess();

        Task<List<string>> GetAllPostalCodes();
    }


    public class ProcessRepository : IProcessRepository
    {
        AppContext _context;

        public ProcessRepository(AppContext context)
        {
            _context = context;
        }
        public async Task<List<string>> GetAllPostalCodes()
        {
            return await _context.PostalCodes.Select(x => x.Code).ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PropertyCrawler.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCrawler.Data.Repositories
{
    public interface IProxyIPsRepository
    {
        Task<IEnumerable<ProxyIp>> GetProxyIpsAsync();
        Task<ProxyIp> GetProxyIpAsync(int id);
        Task<ProxyIp> GetProxyIpDetailsAsync(int id);
        Task<ProxyIp> CreateProxyIp(ProxyIp proxyIP);
        Task<ProxyIp> EditProxyIp(ProxyIp proxyIP);
        Task<ProxyIp> DeleteProxyIp(int id);
    }

    public class ProxyIPsRepository : IProxyIPsRepository
    {
        private AppContext _context;
        public ProxyIPsRepository(AppContext context)
        {
            _context = context;
        }

        public async Task<ProxyIp> GetProxyIpAsync(int id)
        {
            return await _context.ProxyIps.FindAsync(id);
        }

        public async Task<ProxyIp> GetProxyIpDetailsAsync(int id)
        {
            return await _context.ProxyIps.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<ProxyIp>> GetProxyIpsAsync()
        {
            return await _context.ProxyIps.ToListAsync();
        }

        public async Task<ProxyIp> CreateProxyIp(ProxyIp proxyIp)
        {
            _context.Add(proxyIp);
            await _context.SaveChangesAsync();
            return proxyIp;
        }

        public async Task<ProxyIp> EditProxyIp(ProxyIp proxyIP)
        {
            _context.ProxyIps.Update(proxyIP);
            await _context.SaveChangesAsync();
            return proxyIP;
        }

        public async Task<ProxyIp> DeleteProxyIp(int id)
        {
            var proxyIp = await _context.ProxyIps.FindAsync(id);
            _context.ProxyIps.Remove(proxyIp);
            await _context.SaveChangesAsync();
            return proxyIp;
        }

    }
}

using BusinessObject.Model;
using DataAccess.DTO;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public class AdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IAccountRepository _accountRepository;
        public AdminService(IAdminRepository adminRepository, IAccountRepository accountRepository) 
        {
            _adminRepository= adminRepository;
            _accountRepository = accountRepository;
        }
        public async Task<ResponseDTO> ListCategoryAsync()
        {
            var result = await _adminRepository.ListCategory();
            return result;
        }
        public async Task<ResponseDTO> AddCategory(string name)
        {
            var result = await _adminRepository.AddCategory(name);
            return result;
        }
        public async Task<ResponseDTO> AcceptAuctioneerForAdmin(AcceptAutioneerDTO autioneer, string idAuction)
        {
            var result = await _adminRepository.AcceptAuctioneerForAdmin(autioneer, idAuction);
            return result;
        }
        public async Task<ResponseDTO> UpdateCategoryAsync(int id, string Namecategory)
        {
            var result = await _adminRepository.UpdateCategory(id, Namecategory);
            return result;
        }
        public async Task<ResponseDTO> DeleteCategoryAsync(int id)
        {
            var result = await _adminRepository.DeleteCategory(id);
            return result;
        }
        public async Task<List<AuctionnerAdminDTO>> ListAuction(string id, int status)
        {
            var result = await _adminRepository.ListAuction(id, status);
            return result;
        }

        public async Task<ResponseDTO> ProfileUser(string username)
        {
            var result = await _adminRepository.ProfileUser(username);
            return result;
        }
        public async Task<ResponseDTO> ListAccount()
        {
            var listAccount = await _accountRepository.ListAccount();
            return listAccount;
        }
        public async Task<ResponseDTO> AuctioneerDetail(int id)
        {
            var listAccount = await _adminRepository.AuctioneerDetail(id);
            return listAccount;
        }
        public async Task<List<AuctionnerAdminDTO>> ListYourAuctioneerCategoryAdmin(string id, int status, int category)
        {
            var result = await _adminRepository.ListYourAuctioneerCategoryAdmin(id, status, category);
            return result;
        }
        public async Task<List<AuctionnerAdminDTO>> SearchAuctioneerAdmin(string id, string content)
        {
            var result = await _adminRepository.SearchAuctioneerAdmin(id, content);
            return result;
        }
    }
}

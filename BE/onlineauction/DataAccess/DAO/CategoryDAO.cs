using BusinessObject.Context;
using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    /// <summary>
    /// 
    /// </summary>
    public class CategoryDAO
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static CategoryDAO _instance = null;
        /// <summary>
        /// The instance lock
        /// </summary>
        private static readonly object _instanceLock = new object();

        /// <summary>
        /// Prevents a default instance of the <see cref="CategoryDAO"/> class from being created.
        /// </summary>
        private CategoryDAO() { }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static CategoryDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new CategoryDAO();
                    }
                    return _instance;
                }
            }
        }
        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<bool> AddCategory(Category category)
        {
            try
            {
                using(var connect = new ConnectDB())
                {
                    connect.Categorys.Add(category);
                    await connect.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Categories the asynchronous.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<List<Category>> CategoryAsync()
        {
            try
            {
                using(var context = new ConnectDB())
                {
                    var category = await context.Categorys.ToListAsync();
                    return category;
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Updates the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Category not found.
        /// or
        /// </exception>
        public async Task<bool> UpdateCategory(Category category)
        {
            try
            {
                using (var connect = new ConnectDB())
                {
                    var ct = await connect.Categorys.FirstOrDefaultAsync(c => c.CategoryID == category.CategoryID);
                    if (ct == null)
                    {
                        throw new Exception("Category not found.");
                    }
                    ct.NameCategory = category.NameCategory;
                    connect.Entry(ct).State = EntityState.Modified;
                    await connect.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Deletes the category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Category not found.
        /// or
        /// </exception>
        public async Task<bool> DeleteCategory(int id)
        {
            try
            {
                using (var connect = new ConnectDB())
                {
                    // Find the category by its ID
                    var ct = await connect.Categorys.FirstOrDefaultAsync(c => c.CategoryID == id);
                    if (ct == null)
                    {
                        throw new Exception("Category not found.");
                    }

                    // Find if there are any auctions associated with this category
                    var auctions = await connect.AuctionDetails.Where(a => a.CategoryID == id).ToListAsync();
                    if (auctions.Any())
                    {
                        // Update the CategoryID of each auction to null
                        foreach (var auction in auctions)
                        {
                            auction.CategoryID = null;
                            connect.Entry(auction).State = EntityState.Modified; // Mark auction as modified
                        }
                    }

                    // Remove the category
                    connect.Categorys.Remove(ct);
                    await connect.SaveChangesAsync(); // Commit the changes
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exception
                return false;
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                throw new Exception(ex.Message);
            }
        }

    }
}

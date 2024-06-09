namespace MobileRecharge.Infrastructure.Repositories
{
    public class BeneficiaryRepository : IBeneficiaryRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Beneficiarie> _beneficiaryEntities;

        public BeneficiaryRepository(AppDbContext context)
        {
            _dbContext = context;
            _beneficiaryEntities = context.Beneficiaries;

        }

        public async Task<IEnumerable<Beneficiarie>> GetAllBeneficiariesAsync(int userId)
        {
            return await _beneficiaryEntities.Include(i => i.User).Where(i => i.UserId == userId && i.IsActive).ToListAsync();
        }

        public async Task<Beneficiarie?> GetBeneficiaryByIdAsync(int id)
        {
            return await _beneficiaryEntities
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<int> GetBeneficiariesCountAsync(int userId)
        {
            return await _beneficiaryEntities.Where(i => i.UserId == userId && i.IsActive).CountAsync();
        }


        public async Task<int> AddBeneficiaryAsync(int userId, Beneficiarie beneficiary)
        {
            var user = (await _beneficiaryEntities.Include(a=>a.User).FirstOrDefaultAsync(i => i.UserId == userId))!.User;

            if (user == null)
                throw new Exception("User does not exists");

            beneficiary.UserId = userId;
            beneficiary.User = user;
            _beneficiaryEntities.Add(beneficiary);

            await _dbContext.SaveChangesAsync();

            return beneficiary.Id;
        }

        public async Task<bool> UpdateBeneficiaryAsync(int id, Beneficiarie beneficiary)
        {
            var entity = await _beneficiaryEntities.FindAsync(id);

            if (entity == null)
                throw new Exception("Beneficiary does not exists");

            entity.PhoneNumber = beneficiary.PhoneNumber;
            entity.NickName = beneficiary.NickName;

            var result = await _dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteBeneficiaryAsync(int id)
        {
            var entity = await _beneficiaryEntities.FindAsync(id);

            if (entity == null)
                throw new Exception("Beneficiary does not exists");

            entity.IsActive = false;

            var result = await _dbContext.SaveChangesAsync();

            return result > 0;
        }
    }
}


namespace Resources
{
    public interface IConsultantsRepository
    {
        Consultants GetAll();
        int Add(Consultant consultant);
        void Update(Consultant consultant);
    }
}

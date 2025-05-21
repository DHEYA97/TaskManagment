namespace TaskManagment.Core.Specification.EntitySpecification
{
    public class ImageSpecification : BaseSpecification<Entities.Image>
    {
        public ImageSpecification()
        {

        }

        public ImageSpecification(int id) : base(x => x.Id == id)
        {
        }
    }
}

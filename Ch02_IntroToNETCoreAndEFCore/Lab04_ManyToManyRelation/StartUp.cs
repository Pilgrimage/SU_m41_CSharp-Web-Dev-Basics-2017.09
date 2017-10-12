namespace Lab04_ManyToManyRelation
{
    public class StartUp
    {
        public static void Main()
        {
            MtoMDbContext db = new MtoMDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}

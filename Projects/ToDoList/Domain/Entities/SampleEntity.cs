namespace Domain.Entities;

public record SampleEntity
{
    public Guid Id { get; set; }
    public string RandomPropertyOne { get; set; }
    public string RandomPropertyTwo { get; set; }
    public string RandomPropertyThree { get; set; }
    public string RandomPropertyFour { get; set; }
    public string RandomPropertyFive { get; set; }
    public string RandomPropertySix { get; set; }
    public string RandomPropertySeven { get; set; }
    public string RandomPropertyEight { get; set; }
    public string RandomPropertyNine { get; set; }
    public string RandomPropertyTen { get; set; }
    public ToDoItem ToDo { get; set; }
    public ICollection<SecondSample> SecondSamples { get; set; }
}
namespace Domain.Entities;

public record SecondSample
{
    public Guid Id { get; set; }
    public string RandomDeeperPropertyOne { get; set; }
    public string RandomDeeperPropertyTwo { get; set; }
    public string RandomDeeperPropertyThree { get; set; }
    public string RandomDeeperPropertyFour { get; set; }
    public string RandomDeeperPropertyFive { get; set; }
    public string RandomDeeperPropertySix { get; set; }
    public string RandomDeeperPropertySeven { get; set; }
    public string RandomDeeperPropertyEight { get; set; }
    public string RandomDeeperPropertyNine { get; set; }
    public string RandomDeeperPropertyTen { get; set; }
    public SampleEntity SampleEntity { get; set; }
}
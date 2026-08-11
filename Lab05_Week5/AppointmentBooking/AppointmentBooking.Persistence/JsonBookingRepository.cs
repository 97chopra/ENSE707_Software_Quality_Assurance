using System.Text.Json;

namespace AppointmentBooking.Persistence;

public class JsonBookingRepository
{
    private readonly string _filePath;

    public JsonBookingRepository(string filePath)
    {
        _filePath = filePath;
    }

    public void Add(BookingRecord record)
    {
        var records = GetAll().ToList();
        records.Add(record);
        File.WriteAllText(
            _filePath,
            JsonSerializer.Serialize(records));
    }

    public IReadOnlyList<BookingRecord> GetAll()
    {
        if (!File.Exists(_filePath))
            return Array.Empty<BookingRecord>();

        string json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<BookingRecord>>(json)
            ?? new List<BookingRecord>();
    }
}

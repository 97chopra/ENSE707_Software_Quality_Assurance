using ENSE707_AppointmentBooking;

Console.Write("Patient ID: ");
string patientId = Console.ReadLine() ?? "";
Console.Write("Legal name: ");
string legalName = Console.ReadLine() ?? "";
Console.Write("Preferred name (optional): ");
string preferredName = Console.ReadLine() ?? "";
Console.Write("Doctor name: ");
string doctorName = Console.ReadLine() ?? "";
Console.Write("Available slots: ");
int availableSlots = int.Parse(Console.ReadLine() ?? "0");
Console.Write("Days from today (-1, 0, 1, ...): ");
int daysFromToday = int.Parse(Console.ReadLine() ?? "0");

try
{
    var patient = new Patient(patientId, legalName, preferredName);
    var doctor = new Doctor("D001", doctorName, availableSlots);
    var request = new AppointmentRequest(
        patient, doctor, DateTime.Today.AddDays(daysFromToday));
    var service = new AppointmentBookingService();
    BookingResult result = service.BookAppointment(request);
    Console.WriteLine(result.Message);
    Console.WriteLine($"Remaining slots: {doctor.AvailableSlots}");
}
catch (Exception ex)
{
    Console.WriteLine($"Validation error: {ex.Message}");
}
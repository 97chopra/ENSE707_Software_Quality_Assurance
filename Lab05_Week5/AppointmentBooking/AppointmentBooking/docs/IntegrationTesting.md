# Integration Testing

## Why these tests are not equivalent to the existing in-memory service tests

The integration tests in `AppointmentBooking.IntegrationTests` exercise `PersistentAppointmentBookingService`, which wraps `AppointmentBookingService` with real file-system persistence via `JsonBookingRepository`. This is fundamentally different from the existing unit tests in `AppointmentBooking.Tests`, for several reasons:

**Real I/O.** The original unit tests operate entirely in memory — creating `Doctor`, `Patient`, `AppointmentRequest`, and calling `AppointmentBookingService` directly, with no interaction with the file system, database, or any external resource. The integration tests, by contrast, genuinely write JSON to disk (`File.WriteAllText`) and read it back (`File.ReadAllText`), proving the booking is actually persisted, not just held in an object's memory during the test's lifetime.

**Environment state.** Because real files are involved, each test run leaves state behind on disk unless it is cleaned up. This is state external to the test process itself — a fresh repository instance reading the same file must see the same data another instance wrote, which is a genuinely different guarantee than an in-memory object retaining its own state.

**Cleanup.** Unlike the in-memory tests (where the garbage collector reclaims memory automatically once a test ends), the integration tests must explicitly delete their temp file in `[TestCleanup]`. Forgetting this would leave stale files accumulating on disk across test runs.

**Isolation.** The starter project has method-level parallel test execution enabled. If every integration test wrote to the same fixed filename, concurrent test runs could corrupt each other's data or produce flaky, order-dependent failures. Each test in this suite generates a unique temp file path using `Guid.NewGuid()` in `[TestInitialize]`, ensuring tests never collide even when run in parallel.

**Speed.** In-memory unit tests are extremely fast, since no I/O occurs. The integration tests are inherently slower, since they touch the file system, even though this project's tests still complete in milliseconds due to using the local disk.

**Possible failure causes.** The integration tests can fail for reasons the unit tests never could: file permission errors, disk full conditions, a locked file if another process (or a leftover test run) is still holding it open, JSON deserialisation failures if the file becomes corrupted, or path length/character issues. None of these failure modes exist for a purely in-memory object graph.

## Summary

| Aspect | Unit tests (`AppointmentBooking.Tests`) | Integration tests (`AppointmentBooking.IntegrationTests`) |
|---|---|---|
| Dependencies | None (in-memory only) | Real file system |
| Speed | Very fast | Slower (I/O-bound) |
| State | Discarded after test | Persisted to disk unless cleaned up |
| Isolation risk | None | Real risk if tests share a file path |
| Failure causes | Logic errors only | Logic errors + I/O errors, permissions, disk state |
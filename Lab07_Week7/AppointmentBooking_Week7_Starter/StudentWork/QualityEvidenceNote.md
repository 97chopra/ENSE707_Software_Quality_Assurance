# Quality evidence note
## Understanding the measurement context

**Which values are raw observations or measures?**
Each row in `booking-events.csv` (timestamp, validity, outcome, time band, channel, latency) is a raw observation. Each historical row in `release-history.csv` is also a raw observation, recorded from a previous release.

**Which values are targets rather than observations?**
Everything in `quality-targets.json` — the 90% minimum valid success rate, the 1500ms maximum P95 latency, and the 80% minimum peak-hour success rate. These are desired thresholds, not things that were observed happening.

**Is a request rejected because mandatory input is missing a system failure? Explain.**
No. The dataset treats `RejectedInvalid` and `SystemFailure` as separate outcomes. A rejection for invalid input means the system correctly caught bad input before processing it — that is the system behaving as intended, not failing. Counting it as a failure would unfairly penalise correct validation.

**Why is the phrase "booking success rate" incomplete?**
Because it does not state the denominator. Success out of all requests (including invalid ones) tells a very different story than success out of only valid requests. Without naming the population being measured, the phrase cannot be interpreted or compared reliably.

**What important context would be lost if all releases and time bands were combined?**
Combining releases would hide the trend visible in `release-history.csv`: valid success rate, P95 latency and peak-hour success have all steadily worsened over the last four releases. Combining time bands would hide that peak-hour success (80%) is meaningfully worse than normal-hour performance — a concentrated problem would be disguised as a smaller, evenly spread issue.

## Current observations

Summarise the valid-request success rate, P50 and P95 latency, normal-hour success and peak-hour success. State the scope and dataset.

## Comparisons and interpretation

**What does the release trend reveal that the latest snapshot does not?**
The 2.3.0 snapshot alone shows all three targets passing, which looks healthy in isolation. But the four-release trend tells a different story: valid success rate has fallen every release (95% → 93% → 92% → 90%), P50 latency has risen every release (520ms → 560ms → 610ms → 700ms), P95 latency has risen every release (950ms → 1100ms → 1250ms → 1450ms), and peak-hour success has fallen every release (90% → 86% → 84% → 80%). Every indicator is moving in the wrong direction at a steady rate — the snapshot cannot show that current "PASS" results are the last data point before targets would likely be breached next release, not a stable state.

**What does segmentation reveal that the aggregate result conceals?**
The overall valid success rate (90%) hides that normal-hour requests succeed 100% of the time while peak-hour requests only succeed 80% — a 20-point gap. An aggregate view would suggest booking reliability is a uniform, system-wide concern; the segmented view shows the problem is concentrated specifically in peak-hour load, which points to a very different investigation (capacity/scaling) than a uniform reliability issue would.

**Why could average latency appear acceptable while some users still experience long waits?**
An average (or even the P50 median of 700ms) is pulled down by the majority of fast requests and can look fine, while a smaller group of requests — visible only at P95 (1450ms) — take far longer. Averaging blends fast and slow experiences into one number, so a "healthy-looking" average can coexist with a real minority of users experiencing wait times close to the 1500ms limit.

**How does the qualitative feedback complement the numerical evidence?**
The feedback comments corroborate and add texture to the numbers rather than just repeating them. F03 and F05 (peak-hour, web) describe pauses and noticeably slower responses, matching the higher peak-hour latency in the data. F04 (mobile, peak) describes a failure after a long wait with uncertainty over whether a slot was reserved — this points at a possible usability/trust consequence of system failures that the failure-rate number alone doesn't capture. F07 shows the validation working as intended (immediately highlighting a missing field), supporting the earlier point that invalid-input rejections are not failures.

## Decision-oriented indicators

List four indicators you would retain and explain the decision supported by each.
1. **Valid-request success rate (trend, not snapshot)** — supports the decision of whether overall booking reliability needs urgent attention; the trend matters more than any single value.
2. **Peak-hour vs normal-hour success rate (segmented)** — supports prioritising *where* to investigate; shows the problem is concentrated in peak load rather than spread evenly.
3. **P95 latency (trend)** — supports a capacity/performance investigation decision; shows a worst-case experience worsening release over release.
4. **Peak-hour system-failure rate** — directly supports a go/no-go decision on peak-hour capacity work, since it isolates failures that occur once a request is accepted, separate from validation issues.

## Action, inference and unknown

- **Evidence-supported action:** Investigate peak-hour system capacity or load handling, since peak-hour success (80%) is meeting its target only exactly, while the four-release trend shows it declining and P95 latency rising in parallel.
- **Reasonable inference:** The consistent, simultaneous decline across success rate, latency and peak-hour performance over four releases suggests a shared underlying cause (e.g. growing load against fixed capacity) rather than four unrelated issues.
- **Important unknown:** The data does not identify *why* peak-hour requests fail or slow down (e.g. database contention, third-party dependency, insufficient server capacity) — the metrics show a pattern, not a root cause.

## Limitations

Explain the limitations of the synthetic data, the metric definitions and the feedback sample. Do not claim that a metric identifies a cause unless there is separate causal evidence.
The booking-events.csv dataset is synthetic and small (24 observations for the current release, only 10 valid peak-hour requests), so percentages can shift several points with just one additional event and may not reflect real-world variability. The metric definitions rely on outcome and validity labels being recorded consistently — any mislabelling in the source data would silently distort every calculation. The user-feedback.md comments are explicitly a small, constructed sample of 8 entries and are not representative user research; they illustrate possible experiences but cannot be generalised or used to prove causation. None of the metrics in this lab identify a root cause — they indicate where and how much, not why.

## Vanity metric

Identify one attractive but weak metric for this project and explain why it could mislead.
Raw observation count (24 "requests processed") is an attractive but weak metric here. It looks like a straightforward activity indicator, but it says nothing about quality — it doesn't distinguish valid from invalid requests, successes from failures, or fast from slow responses. A team could increase this number simply by generating more traffic while reliability keeps declining, making it easy to report a "busy, healthy-looking" system that is actually getting worse by every real quality indicator.

## Copilot record

**Prompt used:** "Help me implement the ValidSuccessRate method in this C# project. Use only the fields and requirements shown below. Explain the denominator, empty-data behaviour and edge cases before providing code."

**Useful suggestion:** Copilot correctly identified that the denominator should be the count of valid requests (IsValidRequest = true), and it explicitly reasoned through the empty-data case, case-insensitive Outcome comparison, and null-input handling before writing any code — matching the correctness concerns the lab wanted verified.

**Correction / rejection:** Copilot proposed returning `null` for empty data and built a generic, delegate-based `Metrics.ValidSuccessRate<T>` method in a separate `StudentWork` namespace, using `Func<T, bool>` and `Func<T, string?>` selectors instead of the project's actual `BookingEvent` type. I rejected this design: the lab guide's specified implementation throws an `InvalidOperationException` for zero valid requests rather than returning null, and the method needs to live in `AppointmentBooking.Metrics.MetricsCalculator` operating directly on `IEnumerable<BookingEvent>` to match the existing test suite and namespace, not a generic reusable utility.

**Reason:** Silently returning null (or 0) risks being misread downstream as "0% success" rather than "not measurable," which is exactly the misleading behaviour the lab's Q37 warns against — an explicit exception makes the missing-data case impossible to miss. The generic delegate-based version is more reusable in the abstract, but it doesn't match the concrete `BookingEvent` record already defined in this project, and introducing a parallel `StudentWork.Metrics` class would duplicate logic instead of extending the supplied `MetricsCalculator`.


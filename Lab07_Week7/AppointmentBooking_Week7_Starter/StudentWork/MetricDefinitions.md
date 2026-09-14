# Metric definitions

Complete one contract for each required metric. Begin with the quality goal and question, then define the calculation.

## Metric 1 — Valid booking success rate

- **Quality goal:** Improve the reliability of the appointment booking process for patients.
- **Quality question:** What proportion of valid booking requests complete successfully?
- **Metric name and formula:** Valid Booking Success Rate = (valid requests with Outcome = Success) / (all valid requests) × 100
- **Numerator:** Valid requests with Outcome = Success
- **Denominator:** All valid requests (IsValidRequest = true) in scope
- **Included observations:** All valid requests, regardless of whether they succeeded or resulted in a system failure
- **Excluded observations:** Requests rejected because mandatory input was invalid (Outcome = RejectedInvalid)
- **Release, environment and time scope:** Current release (2.3.0), full observation window in booking-events.csv
- **Segment or breakdown:** Normal hours vs peak hours; Web vs Mobile channel
- **Baseline:** Previous release 2.2.0 = 92%; four-release trend in release-history.csv shows a steady decline (95% → 93% → 92% → 90%)
- **Target or warning threshold:** Minimum 90% (quality-targets.json)
- **Decision or action supported:** Investigate booking service reliability if the target is missed, or if the declining trend continues into the next release
- **Limitation or possible misuse:** Does not reveal the root cause of failures. The number could be inflated by reclassifying genuine system failures as invalid-input rejections rather than fixing the underlying issue.

## Metric 2 — P95 booking latency

- **Quality goal:** Ensure booking responses remain fast for the large majority of users, not just on average.
- **Quality question:** Below what response time do 95% of valid booking requests complete?
- **Metric name and formula:** Nearest-rank percentile: sort latency values ascending, rank = ceiling(0.95 × n), select the value at that one-based rank
- **Population and unit:** Latency in milliseconds (LatencyMs), measured only on valid requests
- **Percentile convention:** Nearest-rank (not linear interpolation)
- **Included observations:** Valid requests only (IsValidRequest = true), regardless of outcome
- **Excluded observations:** Invalid requests rejected before processing — their latency reflects how fast rejection happens, not how fast a real booking is processed, so including them would distort the result
- **Release, environment and time scope:** Current release (2.3.0), full observation window in booking-events.csv
- **Segment or breakdown:** Normal hours vs peak hours (peak-hour requests show visibly higher latency)
- **Baseline:** Previous release 2.2.0 P95 = 1250ms; four-release trend shows steady increase (950 → 1100 → 1250 → 1450ms)
- **Target or warning threshold:** Maximum 1500ms (quality-targets.json)
- **Decision or action supported:** Investigate performance/capacity if P95 approaches or exceeds the threshold, or if the rising trend continues
- **Limitation or possible misuse:** A single percentile hides the shape of the full distribution and doesn't show how bad the worst 5% actually is; it also doesn't indicate the cause of slow responses.

## Metric 3 — Peak-hour system-failure rate

- **Quality goal:** Reduce booking failures that occur specifically during periods of high demand.
- **Quality question:** What proportion of valid peak-hour requests result in a system failure?
- **Metric name and formula:** Peak-hour System-Failure Rate = (valid peak-hour requests with Outcome = SystemFailure) / (all valid peak-hour requests) × 100
- **Numerator:** Valid peak-hour requests with Outcome = SystemFailure
- **Denominator:** All valid peak-hour requests (TimeBand = Peak, IsValidRequest = true)
- **Meaning of "peak hour" and "system failure":** Peak hour is a request recorded with TimeBand = Peak, representing a period of higher booking demand. System failure is a valid request that was accepted for processing but did not complete successfully due to a system-side issue, not invalid user input.
- **Included observations:** Valid peak-hour requests only
- **Excluded observations:** Normal-hour requests, and invalid requests that were rejected before ever reaching the booking service
- **Release, environment and time scope:** Current release (2.3.0), peak-hour records in booking-events.csv
- **Baseline:** Implied peak-hour failure rate history (100% − peak success rate in release-history.csv) shows a worsening trend: 10% → 14% → 16% → 20%
- **Target or warning threshold:** No failure rate is stated directly, but it is implied by the 80% minimum peak-hour success target — i.e. a maximum acceptable failure rate of 20%
- **Decision or action supported:** Prioritise investigating peak-hour capacity or load-handling if the failure rate meets or exceeds 20%
- **Limitation or possible misuse:** Based on a small sample (10 valid peak-hour requests in this release), so the rate is volatile and a single extra failure changes it by 10 points. It flags a problem without identifying its cause (e.g. capacity, timeout, race condition).

## Metric audit — Valid booking success rate

**What exactly is measured?**
The percentage of valid booking requests (mandatory input present and correctly formed) that resulted in Outcome = Success, for the current release.

**What scope, denominator, data source and assumptions apply?**
Scope is the current release (2.3.0) using booking-events.csv. The denominator is all requests where IsValidRequest = true. It assumes the Outcome and IsValidRequest fields are recorded accurately and consistently at the point each request is logged.

**Compared with what baseline, target or previous result?**
Compared against the 90% minimum target in quality-targets.json, and against the previous four releases in release-history.csv, which show a steady decline (95% → 93% → 92% → 90%).

**What decision or action does it support?**
Whether booking reliability needs investigation — either because the current value is below target, or because the trend suggests it will breach target soon even if the current value technically passes.

**What missing, invalid or duplicated data could change the result?**
A missing or incorrectly recorded Outcome value would exclude or wrongly include a request. A duplicated event row would double-count a single request, inflating whichever outcome it recorded. Missing IsValidRequest data could shrink or inflate the denominator.

**Could people improve the number without improving the software? How?**
Yes. Genuine system failures could be reclassified as invalid-input rejections (moving them out of the denominator entirely), or the validation logic could be loosened so that borderline requests are marked invalid before they can fail, both of which would raise the reported percentage without fixing anything.

**What important quality concern remains outside this metric?**
It says nothing about *why* requests fail, nothing about response time, and nothing about the concentration of failures in peak hours versus normal hours — a passing overall rate can still hide a serious peak-hour-specific problem.

**Should the metric be communicated as a snapshot, trend, segmented result or threshold comparison? Justify the choice.**
It should be communicated as a **trend with a threshold comparison**, not a snapshot alone. A snapshot showing "90% — PASS" hides that the value has declined every release; showing the trend alongside the target line makes it clear this is a metric approaching failure, not a stable pass.

**Revised metric contract:**
The original "Excluded observations" and "Limitation or possible misuse" fields are revised below to close the gaming risk identified above:

- **Excluded observations (revised):** Requests rejected because mandatory input was invalid (Outcome = RejectedInvalid), *provided the validation rule that rejected them has not changed since the previous release.* Any change to validation logic must be noted alongside the metric so a drop in denominator size is not mistaken for improved input quality.
- **Limitation or possible misuse (revised):** Does not reveal the root cause of failures. The number can be inflated either by reclassifying genuine system failures as invalid-input rejections, or by loosening validation so failing requests are rejected as invalid before they can fail — both must be checked by confirming the validation rules are unchanged before comparing this metric across releases.

**One concrete change made and why:**
I added a condition to the "Excluded observations" field requiring that the validation rule be unchanged across the compared releases, and extended the limitation to explicitly name loosened validation (not just reclassification) as a second way to game the number. This is less ambiguous because the original contract only warned about reclassifying failures — it didn't cover the equally easy tactic of changing what counts as "invalid" in the first place, which the trend data alone can't distinguish from a real improvement.
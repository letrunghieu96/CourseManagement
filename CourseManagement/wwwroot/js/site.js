
// Avoid form resubmission
function avoidFormResubmission() {
	if (window.history.replaceState) window.history.replaceState(null, null, window.location.href);
}
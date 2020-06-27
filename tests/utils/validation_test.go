package utils_test

import (
	"expense-tracker-web-api/utils"
	"testing"
)

var emailTests = []struct {
	mail     string // input
	expected bool   // expected result
}{
	{"aa@aa.a", true},
	{"aa", false},
	// {"aa@aa", false},
	{"aa@aa.", false},
	{"@aa.", false},
}

func TestEmail(t *testing.T) {
	for _, tt := range emailTests {
		actual := utils.ValidateEmail(tt.mail)
		if actual != tt.expected {
			t.Errorf("ValidateEmail(%s): expected %t, actual %t", tt.mail, tt.expected, actual)
		}
	}
}

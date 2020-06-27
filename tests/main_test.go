package tests

import (
	"fmt"
	"os"
	"testing"
)

func TestMain(m *testing.M) {
	fmt.Println("Tests started!")

	// setup()
	code := m.Run()
	// shutdown()

	fmt.Println("Tests finished!")

	os.Exit(code)
}

// func setup() {
// 	os.Setenv("GO_ENV", "TEST")
// }

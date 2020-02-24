package utils

import "log"

// CheckAndPanic ...
func (e error) CheckAndPanic() {
	if e != nil {
		panic(e)
	}
}

// CheckAndLogFatal ...
func (e error) CheckAndLogFatal() {
	if e != nil {
		log.Fatal(e)
	}
}

// CheckAndLog ...
func (e error) CheckAndLog() {
	if e != nil {
		log.Print(e)
	}
}

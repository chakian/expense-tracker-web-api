package utils

import (
	"context"
	"encoding/json"
	"fmt"
	"io/ioutil"

	cloudkms "cloud.google.com/go/kms/apiv1"
	kmspb "google.golang.org/genproto/googleapis/cloud/kms/v1"
)

// Credential ...
type Credential struct {
	DbType       string `json:"db_type"`
	DbHost       string `json:"db_host"`
	DbPort       string `json:"db_port"`
	DbName       string `json:"db_name"`
	DbUser       string `json:"db_user"`
	DbPass       string `json:"db_pass"`
	InstanceName string `json:"instance_name"`
	TokenSecret  string `json:"token_secret"`
}

// Credentials ...
var Credentials Credential

// ReadCredentials ...
func ReadCredentials() {
	fmt.Println("UTIL ReadCredentials Started")
	fmt.Println("Reading credentials from google kms")

	ctx := context.Background()

	projectID := "expense-track-api"
	locationID := "global" //location of key rings
	keyRingID := "Expense-Tracker-Test"
	keyID := "APP-CREDS"
	environmentName := "dev"

	keyURI := fmt.Sprintf(
		"projects/%s/locations/%s/keyRings/%s/cryptoKeys/%s",
		projectID,
		locationID,
		keyRingID,
		keyID)

	credFileName := fmt.Sprintf("./credentials-enc/%s/credentials.json.enc", environmentName)
	credFileContent, err := ioutil.ReadFile(credFileName)
	CheckAndPanic(err)

	client, err := cloudkms.NewKeyManagementClient(ctx)
	CheckAndPanic(err)

	keyringRequest := &kmspb.DecryptRequest{
		Name:       keyURI,
		Ciphertext: credFileContent,
	}
	keyring, err := client.Decrypt(ctx, keyringRequest)
	CheckAndPanic(err)

	err = json.Unmarshal(keyring.GetPlaintext(), &Credentials)
	CheckAndPanic(err)

	fmt.Println("Finished reading credentials")
}

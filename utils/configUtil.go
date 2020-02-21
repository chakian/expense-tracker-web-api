package utils

import (
	"fmt"
	"os"

	"gopkg.in/yaml.v2"
)

// ConfigModel ...
type ConfigModel struct {
	Server struct {
		Environment string `yaml:"environment"`
		TokenSecret string `yaml:"tokensecret"`
	} `yaml:"server"`
	Database struct {
		DbHost       string `yaml:"host"`
		DbPort       string `yaml:"port"`
		DbName       string `yaml:"name"`
		DbUser       string `yaml:"user"`
		DbPass       string `yaml:"pass"`
		InstanceName string `yaml:"instance_name"`
	} `yaml:"database"`
}

// Config ...
var Config ConfigModel

func init() {
	fmt.Println("Started configUtil.init()")

	readYml()

	fmt.Println("Finished configUtil.init()")
}

func readYml() {
	fmt.Println("Reading appconfig.yml")

	f, err := os.Open("appconfig.yml")
	CheckAndPanic(err)
	defer f.Close()

	decoder := yaml.NewDecoder(f)
	err = decoder.Decode(&Config)
	CheckAndPanic(err)
}

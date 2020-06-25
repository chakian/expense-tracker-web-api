package utils

import (
	"fmt"
	"os"

	"github.com/kelseyhightower/envconfig"
	"gopkg.in/yaml.v2"
)

// TODO: Re-Write here with the interface: https://blog.learngoprogramming.com/how-to-mock-in-your-go-golang-tests-b9eee7d7c266

// ConfigModel ...
type ConfigModel struct {
	Server struct {
		Environment string `yaml:"environment" envconfig:"EXP_SERVER_ENVIRONMENT"`
		TokenSecret string `yaml:"tokensecret" envconfig:"EXP_SERVER_TOKEN_SECRET"`
	} `yaml:"server"`
	Database struct {
		DbHost       string `yaml:"host"`
		DbPort       string `yaml:"port"`
		DbName       string `yaml:"name" envconfig:"EXP_DB_NAME"`
		DbUser       string `yaml:"user" envconfig:"EXP_DB_USERNAME"`
		DbPass       string `yaml:"pass" envconfig:"EXP_DB_PASSWORD"`
		InstanceName string `yaml:"instance_name" envconfig:"EXP_DB_INSTANCE_NAME"`
	} `yaml:"database"`
}

// Config ...
var Config ConfigModel

func init() {
	fmt.Println("Started configUtil.init()")

	readYml()
	readEnv()

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

func readEnv() {
	fmt.Println("Reading environment variables")
	err := envconfig.Process("", &Config)
	CheckAndLogFatal(err)
}

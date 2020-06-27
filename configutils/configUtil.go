package configutils

import (
	"fmt"
	"os"

	"github.com/kelseyhightower/envconfig"
	"gopkg.in/yaml.v2"

	u "expense-tracker-web-api/utils"
)

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

// abstractions
// type readEnv func()
// type readYml func()

// // ConfigUtilMethods ...
// type ConfigUtilMethods struct {
// 	readEnvHolder readEnv
// 	readYmlHolder readYml
// }

func readYml() {
	fmt.Println("Reading appconfig.yml")

	f, err := os.Open("appconfig.yml")
	u.CheckAndPanic(err)
	defer f.Close()

	decoder := yaml.NewDecoder(f)
	err = decoder.Decode(&Config)
	u.CheckAndPanic(err)
}

func readEnv() {
	fmt.Println("Reading environment variables")
	err := envconfig.Process("", &Config)
	u.CheckAndLogFatal(err)
}

// Initialize ...
func Initialize() {
	fmt.Println("Started configUtil.Initialize()")
	readEnv()
	readYml()
	fmt.Println("Finished configUtil.Initialize()")
}

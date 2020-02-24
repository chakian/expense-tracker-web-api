package main

import (
	"net/http"

	"fmt"

	"github.com/gorilla/mux"

	"context"
	"log"
	"os"
	"time"

	"expense-tracker-web-api/app"
	"expense-tracker-web-api/controllers"
)

var startupTime time.Time

func main() {
	fmt.Println("Started main")

	setup(context.Background())

	router := mux.NewRouter()
	router.Use(app.JwtAuthentication)

	// Log when an appengine warmup request is used to create the new instance.
	// Warmup steps are taken in setup for consistency with "cold start" instances.
	router.HandleFunc("/_ah/warmup", func(w http.ResponseWriter, r *http.Request) {
		log.Println("warmup done")
	})
	router.HandleFunc("/", indexHandler)

	apiv1 := router.PathPrefix("/api/v1").Subrouter()
	registerRoutesForAPIV1(apiv1)

	apiv1Admin := router.PathPrefix("/api/v1/admin").Subrouter()
	registerRoutesForAPIV1Admin(apiv1Admin)

	port := os.Getenv("PORT")
	if port == "" {
		port = "8000"
		log.Printf("Defaulting to port %s", port)
	}

	log.Printf("Listening on port %s", port)
	if err := http.ListenAndServe(":"+port, router); err != nil {
		log.Fatal(err)
	}
}

func registerRoutesForAPIV1(api *mux.Router) {
	api.HandleFunc("/", indexV1Handler).Methods("GET")

	api.HandleFunc("/user/new", controllers.CreateUser).Methods("POST")
	api.HandleFunc("/user/login", controllers.Authenticate).Methods("POST")
	// api.HandleFunc("/user/{id}/changepassword", controllers.ChangePassword).Methods("PUT")
	// api.HandleFunc("/user/{id}", controllers.UpdateUser).Methods("PUT")
	api.HandleFunc("/user/search/mail", controllers.SearchUsersByEmail).Methods("GET")

	api.HandleFunc("/budget", controllers.CreateBudget).Methods("POST")
	api.HandleFunc("/budget", controllers.GetBudgetsOfUser).Methods("GET")
	api.HandleFunc("/budget/{id}", controllers.GetBudgetByID).Methods("GET")
	api.HandleFunc("/budget/{id}", controllers.UpdateBudget).Methods("PUT")
	// api.HandleFunc("/budget/{id}", controllers.DeleteBudget).Methods("DELETE")
	api.HandleFunc("/budget/adduser", controllers.AddUserToBudget).Methods("POST")
	api.HandleFunc("/budgetuser/approve", controllers.ApproveUserForBudget).Methods("PUT")

	api.HandleFunc("/category", controllers.CreateCategory).Methods("POST")
	api.HandleFunc("/category/{category_id}", controllers.UpdateCategory).Methods("PUT")

	// api.HandleFunc("/account", controllers.CreateAccount).Methods("POST")
	// api.HandleFunc("/account", controllers.UpdateAccount).Methods("PUT")

	// api.HandleFunc("/transaction", controllers.CreateTransaction).Methods("POST")
	// api.HandleFunc("/transaction", controllers.UpdateTransaction).Methods("PUT")
	// api.HandleFunc("/transaction", controllers.DeleteTransaction).Methods("DELETE")
}

func registerRoutesForAPIV1Admin(api *mux.Router) {
	api.HandleFunc("/", indexV1Handler).Methods("GET") // TODO: Change Handler
}

func setup(ctx context.Context) error {
	startupTime = time.Now()
	return nil
}

func indexHandler(w http.ResponseWriter, r *http.Request) {
	if r.URL.Path != "/" {
		http.NotFound(w, r)
		return
	}
	uptime := time.Since(startupTime).Seconds()
	fmt.Fprintf(w, "Hello, World! Uptime: %.2fs\n", uptime)
}

func indexV1Handler(w http.ResponseWriter, r *http.Request) {
	if r.URL.Path != "/api/v1/" {
		http.NotFound(w, r)
		return
	}
	uptime := time.Since(startupTime).Seconds()
	fmt.Fprintf(w, "API Version 1. Uptime: %.2fs\n", uptime)
}

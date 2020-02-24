ALTER TABLE `expense_tracker`.`budget_user` 
ADD UNIQUE INDEX `budget_user_budget_id_user_id_uni` (`budget_id` ASC, `user_id` ASC) VISIBLE;
;

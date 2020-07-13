ALTER TABLE `expense_tracker`.`user` 
ADD COLUMN `default_budget_id` INT UNSIGNED NULL AFTER `active_flag`,
ADD INDEX `user_default_budget_id_idx` (`default_budget_id` ASC) VISIBLE;
;
ALTER TABLE `expense_tracker`.`user` 
ADD CONSTRAINT `user_default_budget_id`
  FOREIGN KEY (`default_budget_id`)
  REFERENCES `expense_tracker`.`budget` (`budget_id`)
  ON DELETE RESTRICT
  ON UPDATE NO ACTION;

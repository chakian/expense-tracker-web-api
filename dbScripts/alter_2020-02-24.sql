ALTER TABLE `expense_tracker`.`budget_user` 
ADD UNIQUE INDEX `budget_user_budget_id_user_id_uni` (`budget_id` ASC, `user_id` ASC) VISIBLE;
;


ALTER TABLE `expense_tracker`.`budget_category` 
DROP FOREIGN KEY `budget_category_parent_category_id`;
ALTER TABLE `expense_tracker`.`budget_category` 
CHANGE COLUMN `parent_category_id` `parent_category_id` INT UNSIGNED NULL DEFAULT NULL ;
ALTER TABLE `expense_tracker`.`budget_category` 
ADD CONSTRAINT `budget_category_parent_category_id`
  FOREIGN KEY (`parent_category_id`)
  REFERENCES `expense_tracker`.`budget_category` (`budget_category_id`);


ALTER TABLE `expense_tracker`.`budget_account` 
ADD COLUMN `budget_id` INT UNSIGNED NOT NULL AFTER `budget_account_id`,
ADD INDEX `budget_account_budget_id_idx` (`budget_id` ASC) VISIBLE;
;
ALTER TABLE `expense_tracker`.`budget_account` 
ADD CONSTRAINT `budget_account_budget_id`
  FOREIGN KEY (`budget_id`)
  REFERENCES `expense_tracker`.`budget` (`budget_id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

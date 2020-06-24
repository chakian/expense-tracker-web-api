CREATE TABLE `account_type` (
  `account_type_id` int unsigned NOT NULL,
  `account_type_name` varchar(255) NOT NULL,
  PRIMARY KEY (`account_type_id`),
  UNIQUE KEY `account_type_id_UNIQUE` (`account_type_id`),
  UNIQUE KEY `account_type_name_UNIQUE` (`account_type_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `user` (
  `user_id` int unsigned NOT NULL AUTO_INCREMENT,
  `email` varchar(255) NOT NULL,
  `user_name` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `register_date` datetime NOT NULL,
  `email_confirmed_flag` tinyint unsigned NOT NULL DEFAULT '0',
  `active_flag` tinyint unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `user_id_UNIQUE` (`user_id`),
  UNIQUE KEY `email_UNIQUE` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4;

CREATE TABLE `budget` (
  `budget_id` int unsigned NOT NULL AUTO_INCREMENT,
  `budget_name` varchar(255) NOT NULL,
  `active_flag` tinyint unsigned DEFAULT '1',
  `insert_user_id` int unsigned NOT NULL,
  `insert_time` datetime NOT NULL,
  `update_user_id` int unsigned DEFAULT NULL,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`budget_id`),
  UNIQUE KEY `budget_id_UNIQUE` (`budget_id`),
  KEY `budget_insert_user_id_idx` (`insert_user_id`),
  KEY `budget_update_user_id_idx` (`update_user_id`),
  CONSTRAINT `budget_insert_user_id` FOREIGN KEY (`insert_user_id`) REFERENCES `user` (`user_id`),
  CONSTRAINT `budget_update_user_id` FOREIGN KEY (`update_user_id`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

CREATE TABLE `budget_account` (
  `budget_account_id` int unsigned NOT NULL AUTO_INCREMENT,
  `account_name` varchar(255) NOT NULL,
  `account_type_id` int unsigned NOT NULL,
  `active_flag` tinyint unsigned NOT NULL DEFAULT '1',
  `insert_user_id` int unsigned NOT NULL,
  `insert_time` datetime NOT NULL,
  `update_user_id` int unsigned DEFAULT NULL,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`budget_account_id`),
  UNIQUE KEY `budget_account_id_UNIQUE` (`budget_account_id`),
  KEY `budget_account_account_type_id_idx` (`account_type_id`),
  KEY `budget_account_insert_user_id_idx` (`insert_user_id`),
  KEY `budget_account_update_user_id_idx` (`update_user_id`),
  CONSTRAINT `budget_account_account_type_id` FOREIGN KEY (`account_type_id`) REFERENCES `account_type` (`account_type_id`),
  CONSTRAINT `budget_account_insert_user_id` FOREIGN KEY (`insert_user_id`) REFERENCES `user` (`user_id`),
  CONSTRAINT `budget_account_update_user_id` FOREIGN KEY (`update_user_id`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `budget_category` (
  `budget_category_id` int unsigned NOT NULL AUTO_INCREMENT,
  `category_name` varchar(255) NOT NULL,
  `budget_id` int unsigned NOT NULL,
  `parent_category_id` int unsigned NOT NULL DEFAULT '0',
  `active_flag` tinyint unsigned NOT NULL DEFAULT '1',
  `insert_user_id` int unsigned NOT NULL,
  `insert_time` datetime NOT NULL,
  `update_user_id` int unsigned DEFAULT NULL,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`budget_category_id`),
  UNIQUE KEY `budget_category_id_UNIQUE` (`budget_category_id`),
  KEY `budget_category_budget_id_idx` (`budget_id`),
  KEY `budget_category_parent_category_id_idx` (`parent_category_id`),
  KEY `budget_category_insert_user_id_idx` (`insert_user_id`),
  KEY `budget_category_update_user_id_idx` (`update_user_id`),
  CONSTRAINT `budget_category_budget_id` FOREIGN KEY (`budget_id`) REFERENCES `budget` (`budget_id`),
  CONSTRAINT `budget_category_insert_user_id` FOREIGN KEY (`insert_user_id`) REFERENCES `user` (`user_id`),
  CONSTRAINT `budget_category_parent_category_id` FOREIGN KEY (`parent_category_id`) REFERENCES `budget_category` (`budget_category_id`),
  CONSTRAINT `budget_category_update_user_id` FOREIGN KEY (`update_user_id`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `budget_user` (
  `budget_user_id` int unsigned NOT NULL AUTO_INCREMENT,
  `budget_id` int unsigned NOT NULL,
  `user_id` int unsigned NOT NULL,
  `user_approved_flag` tinyint unsigned NOT NULL DEFAULT '0',
  `active_flag` tinyint unsigned NOT NULL DEFAULT '1',
  `insert_user_id` int unsigned NOT NULL,
  `insert_time` datetime NOT NULL,
  `update_user_id` int unsigned DEFAULT NULL,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`budget_user_id`),
  UNIQUE KEY `budget_user_id_UNIQUE` (`budget_user_id`),
  KEY `budget_user_budget_id_idx` (`budget_id`),
  KEY `budget_user_user_id_idx` (`user_id`),
  KEY `budget_user_insert_user_id_idx` (`insert_user_id`),
  KEY `budget_user_update_user_id_idx` (`update_user_id`),
  CONSTRAINT `budget_user_budget_id` FOREIGN KEY (`budget_id`) REFERENCES `budget` (`budget_id`),
  CONSTRAINT `budget_user_insert_user_id` FOREIGN KEY (`insert_user_id`) REFERENCES `user` (`user_id`),
  CONSTRAINT `budget_user_update_user_id` FOREIGN KEY (`update_user_id`) REFERENCES `user` (`user_id`),
  CONSTRAINT `budget_user_user_id` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `transaction_header` (
  `transaction_header_id` int unsigned NOT NULL AUTO_INCREMENT,
  `budget_id` int unsigned NOT NULL,
  `source_account_id` int unsigned NOT NULL,
  `target_account_id` int unsigned DEFAULT NULL,
  `transaction_date` datetime NOT NULL,
  `category_id` int unsigned DEFAULT NULL,
  `multiple_item_flag` tinyint unsigned NOT NULL DEFAULT '0',
  `amount` decimal(12,2) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  `active_flag` tinyint unsigned NOT NULL DEFAULT '1',
  `insert_user_id` int unsigned NOT NULL,
  `insert_time` datetime NOT NULL,
  `update_user_id` int unsigned DEFAULT NULL,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`transaction_header_id`),
  UNIQUE KEY `transaction_header_id_UNIQUE` (`transaction_header_id`),
  KEY `transaction_header_source_account_id_idx` (`source_account_id`),
  KEY `transaction_header_target_account_id_idx` (`target_account_id`),
  KEY `transaction_header_budget_id_idx` (`budget_id`),
  KEY `transaction_header_insert_user_id_idx` (`insert_user_id`),
  KEY `transaction_header_update_user_id_idx` (`update_user_id`),
  KEY `transaction_header_category_id_idx` (`category_id`),
  CONSTRAINT `transaction_header_budget_id` FOREIGN KEY (`budget_id`) REFERENCES `budget` (`budget_id`),
  CONSTRAINT `transaction_header_category_id` FOREIGN KEY (`category_id`) REFERENCES `budget_category` (`budget_category_id`),
  CONSTRAINT `transaction_header_insert_user_id` FOREIGN KEY (`insert_user_id`) REFERENCES `user` (`user_id`),
  CONSTRAINT `transaction_header_source_account_id` FOREIGN KEY (`source_account_id`) REFERENCES `budget_account` (`budget_account_id`),
  CONSTRAINT `transaction_header_target_account_id` FOREIGN KEY (`target_account_id`) REFERENCES `budget_account` (`budget_account_id`),
  CONSTRAINT `transaction_header_update_user_id` FOREIGN KEY (`update_user_id`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `transaction_detail` (
  `transaction_detail_id` int unsigned NOT NULL AUTO_INCREMENT,
  `transaction_header_id` int unsigned NOT NULL,
  `category_id` int unsigned NOT NULL,
  `amount` decimal(12,2) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`transaction_detail_id`),
  UNIQUE KEY `transaction_detail_id_UNIQUE` (`transaction_detail_id`),
  KEY `transaction_detail_transaction_header_id_idx` (`transaction_header_id`),
  KEY `transaction_detail_category_id_idx` (`category_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

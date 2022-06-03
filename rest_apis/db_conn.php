<?php
error_reporting(E_ALL ^ E_WARNING); 
//adjust names
	class DBobject {
		private $host;
		private $user;
		private $pass;
		private $db;
		private $mysqli;

		public function __construct() {
			$this->host = 'localhost:3307';
			$this->user = 'root';
			$this->pass = '';
			$this->db = 'lab_exer5';
			$this->mysqli = new mysqli($this->host, $this->user, $this->pass, $this->db) or die($this->mysqli->error);

			if($this->mysqli->connect_error){
				die("Connection failed".$this->mysqli->connect_error);
			}
			
		}

		public function add_data($name, $school, $gender, $country) {
			$exists = $this->check_data($name);

			if ($exists){ return; }

			$query = "INSERT INTO `user_info`(`name`, `school`, `gender`, `country`) VALUES ('$name', '$school', '$gender', '$country')";
			mysqli_query($this->mysqli, $query);
		}

		public function update_data($name, $school, $gender, $country) {
			$query = "UPDATE `user_info` SET `name`='$name',`school`='$school',`gender`='$gender',`country`='$country'
					  WHERE UPPER(`name`) LIKE UPPER('$name')";
			mysqli_query($this->mysqli, $query);
		}

		public function delete_data($name) {
			$query = "DELETE FROM `user_info` WHERE UPPER(`name`) LIKE UPPER('$name')";
			mysqli_query($this->mysqli, $query);
		}

		public function search_data($name) {
			$query = "SELECT DISTINCT * FROM `user_info` WHERE UPPER(`name`) LIKE UPPER('$name')";

			$result = mysqli_query($this->mysqli, $query);
			$json = array();
			while($row = mysqli_fetch_assoc($result)) {
				$json = $row;
			}

			return json_encode($json);
		}

		public function check_data($name){
			$query = "SELECT DISTINCT `name` FROM `user_info` WHERE UPPER(`name`) LIKE UPPER('$name')";
			$result = mysqli_query($this->mysqli, $query);

			if($result->num_rows == 0) {
				return FALSE;
			}
			return TRUE;
		}

		public function validate_user($user, $pass) {
			$query = "SELECT * FROM `login_info` WHERE `username` = '$user' AND `password` = '$pass'";
			$result = mysqli_query($this->mysqli,$query);

			if($result->num_rows == 0){
				return FALSE;
			}
			return TRUE;
		}
	}
	
?>
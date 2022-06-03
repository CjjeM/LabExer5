<?php
    include 'db_conn.php';

    $db_conn = new DBobject;

    $name = $_GET['name'];
    $school = $_GET['school'];
    $gender = $_GET['gender'];
    $country = $_GET['country'];

    $db_conn->update_data($name, $school, $gender, $country);

    die();
?>
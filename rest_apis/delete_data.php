<?php
    include 'db_conn.php';

    $db_conn = new DBobject;

    $name = $_GET['name'];

    $db_conn->delete_data($name);

    die();

?>
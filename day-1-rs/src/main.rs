use std::error::Error;
use std::fs::File;
use std::io::{self, BufRead};

const SEARCH_VALUE: i32 = 2020;

fn main() {
    let input_path = match std::env::args().nth(1) {
        Some(path) => path,
        None => {
            println!("Please supply an input data text file");
            return;
        }
    };

    match load_expenses(&input_path) {
        Ok(expenses) => {
            part_one(&expenses);
            part_two(&expenses);
        }
        Err(e) => println!("Failed to load expenses from file: {}", e),
    }
}

fn part_one(expenses: &[i32]) {
    match find_sum_pair(&expenses, SEARCH_VALUE) {
        Some((a, b)) => println!("Part 1: Answer: {}", a * b),
        None => println!("Part 1: Couldn't find the values"),
    }
}

fn part_two(expenses: &[i32]) {
    match find_sum_triplet(&expenses, SEARCH_VALUE) {
        Some((a, b, c)) => println!("Part 2: Answer: {}", a * b * c),
        None => println!("Part 2: Couldn't find the values"),
    }
}

fn load_expenses(filepath: &str) -> Result<Vec<i32>, Box<dyn Error>> {
    let input_file = File::open(filepath)?;
    let reader = io::BufReader::new(input_file);
    reader
        .lines()
        .map(|line| {
            let line = line?;
            let num = line.parse::<i32>()?;
            Ok(num)
        })
        .collect()
}

fn find_sum_pair(expenses: &[i32], target_sum: i32) -> Option<(i32, i32)> {
    for index in 0..expenses.len() {
        let rest = &expenses[index + 1..];
        let current = expenses[index];
        for other in rest {
            let sum = current + other;
            if sum == target_sum {
                return Some((current, *other));
            }
        }
    }
    None
}

fn find_sum_triplet(expenses: &[i32], target_sum: i32) -> Option<(i32, i32, i32)> {
    for index in 0..expenses.len() {
        let rest = &expenses[index + 1..];
        let current = expenses[index];
        let remaining_sum = target_sum - current;
        if let Some((a, b)) = find_sum_pair(&rest, remaining_sum) {
            return Some((current, a, b));
        }
    }
    None
}

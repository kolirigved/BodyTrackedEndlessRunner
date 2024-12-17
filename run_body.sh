#!/bin/bash

# Get the script's location
script_dir="$(cd "$(dirname "$0")" && pwd)"

# Change to the target directory
cd "$script_dir" || exit

# Source the virtual environment
source venv/bin/activate

# Run the Python script
python3.10 Body.py


import re

def cleanup_formatting(input_file, output_file):
    with open(input_file, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Split into lines
    lines = content.split('\n')
    
    # Process each line
    cleaned_lines = []
    for line in lines:
        # Remove trailing spaces
        cleaned_line = line.rstrip()
        
        # If line contains "fCost", add two spaces at the end
        if "fCost" in cleaned_line:
            cleaned_line += "  "
        
        cleaned_lines.append(cleaned_line)
    
    # Join lines back together
    cleaned_content = '\n'.join(cleaned_lines)
    
    # Write to output file
    with open(output_file, 'w', encoding='utf-8') as f:
        f.write(cleaned_content)
    
    print(f"Cleaned formatting saved to {output_file}")

if __name__ == "__main__":
    cleanup_formatting("Assets/Scripts/Core.cs", "Assets/Scripts/Core_cleaned.cs") 
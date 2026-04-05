# frozen_string_literal: true

Gem::Specification.new do |s|
  s.name          = 'regilattice'
  s.version       = File.read(File.join(__dir__, 'lib', 'regilattice', 'version.rb'))
                       .match(/VERSION\s*=\s*'([^']+)'/)[1]
  s.authors       = ['Yair Rajwan']
  s.email         = []
  s.summary       = 'RegiLattice — Windows registry tweak toolkit'
  s.description   = '7,189 curated Windows 10/11 registry tweaks across 23 categories. ' \
                    'Debloat, privacy, performance, security, and more. GUI + CLI.'
  s.homepage      = 'https://github.com/RajwanYair/RegiLattice'
  s.license       = 'MIT'

  s.required_ruby_version = '>= 2.7.0'

  s.metadata = {
    'homepage_uri'    => s.homepage,
    'source_code_uri' => s.homepage,
    'bug_tracker_uri' => "#{s.homepage}/issues"
  }

  s.files       = Dir['lib/**/*', 'bin/*']
  s.executables = ['regilattice']
  s.require_paths = ['lib']
end
